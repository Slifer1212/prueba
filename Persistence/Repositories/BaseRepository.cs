using Application.Interfaces;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _entities;
    protected ILogger<BaseRepository<T>> Logger;

    public BaseRepository(ApplicationDbContext context, ILogger<BaseRepository<T>> logger)
    {
        _context = context;
        Logger = logger;
        _entities = _context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _entities.Where(t => t.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _entities.AddAsync(entity ,  cancellationToken);
        Logger?.LogInformation($"Added entity: {entity.Id}");
    }

    public void Update(T entity)
    {
        _entities.Update(entity);
        Logger.LogInformation("Entity was updated");
    }

    public void Delete(int id)
    {
        var entity = _entities.Find(id);

        if (entity == null)
        {
            Logger.LogError($"Entity with id: {id} not found");
            return;
        }
        
        _entities.Remove(entity);
    }
}