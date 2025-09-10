using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper;
using Domain.Database;

namespace Infrastructure.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;
    private readonly string _tableName;

    public GenericRepository(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
        _tableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name;
    }

    public async Task<int> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var columns = GetColumns(excludeKey: true);
        var values = GetPropertyNames(excludeKey: true);

        var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";

        return await _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, _transaction, cancellationToken: cancellationToken)
        );
    }

    public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var key = GetKeyName();
        var setClause = GetSetClause(excludeKey: true);

        var sql = $"UPDATE {_tableName} SET {setClause} WHERE {key} = @{key}";

        return await _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, _transaction, cancellationToken: cancellationToken)
        );
    }

    public async Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default)
    {
        var key = GetKeyName();
        var sql = $"DELETE FROM {_tableName} WHERE {key} = @Id";

        return await _connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, _transaction, cancellationToken: cancellationToken)
        );
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        var key = GetKeyName();
        var sql = $"SELECT * FROM {_tableName} WHERE {key} = @Id";

        return await _connection.QueryFirstOrDefaultAsync<T>(
            new CommandDefinition(sql, new { Id = id }, _transaction, cancellationToken: cancellationToken)
        );
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName}";
        return await _connection.QueryAsync<T>(
            new CommandDefinition(sql, transaction: _transaction, cancellationToken: cancellationToken)
        );
    }

    public async Task<IEnumerable<T>> GetDataAsync(string query, object? param = null, CancellationToken cancellationToken = default)
    {
        return await _connection.QueryAsync<T>(
            new CommandDefinition(query, param, _transaction, cancellationToken: cancellationToken)
        );
    }

    #region Helpers
    private string GetKeyName()
    {
        return typeof(T).GetProperties()
            .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Name
            ?? throw new Exception("No key property named 'Id' found.");
    }

    private string GetColumns(bool excludeKey)
    {
        return string.Join(", ", typeof(T).GetProperties()
        .Where(p => !excludeKey || !p.Name.Equals(GetKeyName(), StringComparison.OrdinalIgnoreCase))
        .Select(p =>
        {
            var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
            return columnAttr?.Name ?? p.Name;
        }));
    }

    private string GetPropertyNames(bool excludeKey)
    {
        return string.Join(", ", typeof(T).GetProperties()
            .Where(p => !excludeKey || !p.Name.Equals(GetKeyName(), StringComparison.OrdinalIgnoreCase))
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttr?.Name ?? p.Name;
                return "@" + p.Name; // dùng tên property để binding với Dapper
            }));
    }

    private string GetSetClause(bool excludeKey)
    {
        return string.Join(", ", typeof(T).GetProperties()
            .Where(p => !excludeKey || !p.Name.Equals(GetKeyName(), StringComparison.OrdinalIgnoreCase))
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttr?.Name ?? p.Name;
                return $"{columnName} = @{p.Name}";
            }));
    }

    #endregion
}
