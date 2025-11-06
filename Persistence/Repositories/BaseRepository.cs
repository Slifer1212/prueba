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
        await _context.SaveChangesAsync(cancellationToken);
        Logger?.LogInformation($"Added entity: {entity.Id}");
    }

    public async Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
       _entities.Update(entity);
       await _context.SaveChangesAsync(cancellationToken);
       Logger?.LogInformation("Entity was updated");
       return entity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _entities.FindAsync(id, cancellationToken);
        if (entity == null)
        {
            Logger.LogWarning($"Entity with Id: {id} was not found");
            return;
        }
        _entities.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}