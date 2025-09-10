using Infrastructure.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }

        IGenericRepository<T> GetRepository<T>() where T : class;

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string spName, object? parameters = null, CancellationToken cancellationToken = default);
    }
}
