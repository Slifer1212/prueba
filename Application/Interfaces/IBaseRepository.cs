using Domain.Base;

namespace Application.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id , CancellationToken cancellationToken = default);
    Task AddAsync(T entity , CancellationToken cancellationToken = default);
    Task<T?> UpdateAsync(T entity , CancellationToken cancellationToken = default);
    Task DeleteAsync(int id , CancellationToken cancellationToken = default);
}