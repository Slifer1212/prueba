using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface ITaskRepository : IBaseRepository<TodoTask>
{
    Task<List<TodoTask>>GetAllTaskCompletedAsync();
    Task <List<TodoTask>> GetTaskByPriorityAsync(TaskPriority priority, CancellationToken cancellationToken = default);

    Task<List<TodoTask>> GetOrderTasksByPriorityAsync(
        CancellationToken cancellationToken = default); 
        Task <List<TodoTask>> GetTaskByUserAsync(int userId, CancellationToken cancellationToken = default);
    
}