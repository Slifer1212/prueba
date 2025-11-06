using Domain.Enums;

namespace Application.Dto;

public class ReadTodoTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; } = "No description";
    public DateTime? DueDate { get; set; }
    public bool Completed { get; set; } 
    public TaskPriority TaskPriority { get; set; }
    
    public int UserId { get; set; }
}