using Domain.Base;
using Domain.Enums;

namespace Domain.Entities;

public class TodoTask : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Completed { get; set; } = false;
    public TaskPriority TaskPriority { get; set; }
    
    //Foreign Key
    public int UserId { get; set; }
    //navegation propertie
    public User User { get; set; }
}