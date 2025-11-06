using Application.Dto;
using Application.Dto.TodoDTos;
using Domain.Entities;

namespace Application.IServices;

public interface ITaskService : IBaseService<TodoTask ,
    CreateTodoTaskDto, UpdateTodoTaskDto, ReadTodoTaskDto>
{
    Task<ReadTodoTaskDto> CompleteTaskAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ReadTodoTaskDto>> GetExpiredTasksAsync(CancellationToken cancellationToken = default);

    Task<List<ReadTodoTaskDto>> OrderTaskByPriorityAsync(CancellationToken cancellationToken = default);
}