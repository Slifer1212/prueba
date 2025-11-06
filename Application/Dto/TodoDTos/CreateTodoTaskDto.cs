using Domain.Enums;

namespace Application.Dto.TodoDTos;

public class CreateTodoTaskDto
{
    public int UserId{ get; set; }
    public string Title { get; set; }
    public string? Description { get; set; } = "No description";
    public DateTime? DueDate { get; set; }
    public TaskPriority TaskPriority { get; set; }
}