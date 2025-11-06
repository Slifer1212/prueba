using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class TaskRepository : BaseRepository<TodoTask>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context, ILogger<BaseRepository<TodoTask>> logger) : base(context, logger)
    {
    }

    public async Task<List<TodoTask>> GetAllTaskCompletedAsync()
    {
        return await _entities.Where(e => e.Completed == true).ToListAsync();
    }

    public async Task<List<TodoTask>> GetTaskByPriorityAsync(TaskPriority priority,
        CancellationToken cancellationToken = default)
    {
        return await _entities
            .Where(e => e.TaskPriority == priority).ToListAsync(cancellationToken);
    }

    public async Task<List<TodoTask>> GetOrderTasksByPriorityAsync(CancellationToken cancellationToken = default)
    {
        return await _entities
            .OrderByDescending(e => e.TaskPriority).ToListAsync(cancellationToken);   
    }

    public async Task<List<TodoTask>> GetTaskByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _entities.Where(e => e.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<bool> MarkTaskAsCompletedAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _entities.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    
        if (entity == null)
            return false;
    
        entity.Completed = true;
        _entities.Update(entity);
    
        return true;
    }
}