using Application.Dto;
using Application.Dto.TodoDTos;
using Application.Interfaces;
using Application.IServices;
using Application.Validations;
using Domain.Entities;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class TaskService : BaseService<TodoTask,
CreateTodoTaskDto, UpdateTodoTaskDto, ReadTodoTaskDto> ,  ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private CreateTaskDtoValidator _validator;
    private UpdateTaskDtoValidator _updateValidator;
    

    public TaskService(IMapper mapper, IBaseRepository<TodoTask> repository, ILogger<BaseService<TodoTask, CreateTodoTaskDto, UpdateTodoTaskDto, ReadTodoTaskDto>> logger, ITaskRepository repository2, CreateTaskDtoValidator validator, UpdateTaskDtoValidator updateValidator, IUserRepository userRepository) : base(mapper, repository, logger)
    {
        _taskRepository = repository2;
        _validator = validator;
        _updateValidator = updateValidator;
        _userRepository = userRepository;
    }

    public override async Task<ReadTodoTaskDto> CreateAsync(CreateTodoTaskDto dto, CancellationToken cancellationToken = default)
    {
        var existUser = await _userRepository.GetByIdAsync(dto.UserId);
        if (existUser == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        _validator.ValidateAndThrow(dto);

        var task = new TodoTask()
        {
            Title = dto.Title,
            Description = dto.Description,
            UserId = dto.UserId,
            DueDate = dto.DueDate,
            TaskPriority = dto.TaskPriority,
        };
        
        await _taskRepository.AddAsync(task, cancellationToken);
        Logger.LogInformation($"Task created with id {task.Id}");
        return task.Adapt<ReadTodoTaskDto>();
    }

    public async override Task<ReadTodoTaskDto?> UpdateAsync(int id, UpdateTodoTaskDto dto, CancellationToken cancellationToken = default)
    {
        
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        
        _updateValidator.ValidateAndThrow(dto);
        
        existingTask.Title = dto.Title;
        existingTask.Description = dto.Description;
        existingTask.DueDate = dto.DueDate;
        if (existingTask.TaskPriority != null) existingTask.TaskPriority = dto.Priority.Value!;
        existingTask.UpdatedAt = DateTime.UtcNow;
        if(existingTask.Completed != null) existingTask.Completed = dto.Completed.Value;
        
        await _taskRepository.UpdateAsync(existingTask, cancellationToken);
        Logger.LogInformation($"Task {existingTask.Id} updated");
        return existingTask.Adapt<ReadTodoTaskDto>();
    }
    

    public async Task<ReadTodoTaskDto> CompleteTaskAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        if (task.Completed)
        {
            throw new Exception($"Task {task.Id} is already completed");
        }
        task.Completed = true;
        task.UpdatedAt = DateTime.UtcNow;
        
        await _taskRepository.UpdateAsync(task, cancellationToken);
        Logger.LogInformation($"Task {task.Id} completed");
        return task.Adapt<ReadTodoTaskDto>();
    }

    public Task<List<ReadTodoTaskDto>> GetExpiredTasksAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ReadTodoTaskDto>> OrderTaskByPriorityAsync(CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetOrderTasksByPriorityAsync(cancellationToken);
        return task.Adapt<List<ReadTodoTaskDto>>();
    }
}