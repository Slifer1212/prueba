using Domain.Base;

namespace Application.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id , CancellationToken cancellationToken = default);
    Task AddAsync(T entity , CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(int id);
}