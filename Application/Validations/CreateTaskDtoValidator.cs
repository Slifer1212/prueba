using Application.Dto;
using Application.Dto.TodoDTos;
using Domain.Entities;
using FluentValidation;

namespace Application.Validations;

public class CreateTaskDtoValidator : AbstractValidator<CreateTodoTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty()
            .WithMessage("Please enter a user id");

        RuleFor(t => t.Title).NotEmpty().NotNull()
            .MinimumLength(5).MaximumLength(25)
            .WithMessage("The title must be between 5 and 25 characters");
        RuleFor(t => t.Description).MaximumLength(50).MinimumLength(10)
            .WithMessage("The description must be between 10 and 50 characters");
        
        RuleFor(t => t.DueDate)
            .Must(BeAValidDate)
            .WithMessage("Due date cannot be in the past.");     
        
        RuleFor(t => t.TaskPriority).NotNull().NotEmpty()
            .WithMessage("TaskPriority cannot be null or empty.");
    }
    protected bool BeAValidDate(DateTime? dueDate)
    {

        if (!dueDate.HasValue)
            return false; 

        return dueDate.Value.Date >= DateTime.Now.Date;
    }

}