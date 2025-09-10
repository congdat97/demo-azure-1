using Dapper;
using Domain.Database;
using Infrastructure.GenericRepository;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Model;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbConnection _connection;
        private DbTransaction? _transaction;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool _disposed;

        public DbConnection Connection => _connection;
        public DbTransaction Transaction => _transaction!;

        public UnitOfWork(ConnectDatabase connectDatabase)
        {
            _connection = new NpgsqlConnection(connectDatabase.DefaultConnection);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync(cancellationToken);

            if (_transaction == null || _transaction.Connection == null)
            {
                _transaction = await _connection.BeginTransactionAsync(cancellationToken);
            }
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (_repositories.TryGetValue(type, out var repo))
                return (IGenericRepository<T>)repo;

            var newRepo = new GenericRepository<T>(_connection, _transaction);
            _repositories[type] = newRepo;
            return newRepo;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = await _connection.BeginTransactionAsync(cancellationToken); // reset
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = await _connection.BeginTransactionAsync(cancellationToken); // reset
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_transaction != null)
                    await _transaction.DisposeAsync();

                if (_connection.State != System.Data.ConnectionState.Closed)
                    await _connection.CloseAsync();

                await _connection.DisposeAsync();

                _disposed = true;
            }
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string spName, object? parameters = null, CancellationToken cancellationToken = default)
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(cancellationToken);

            var result = await _connection.QueryAsync<T>(
                sql: spName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
