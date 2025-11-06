using Application.Dto;
using Application.Dto.TodoDTos;
using FluentValidation;

namespace Application.Validations;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTodoTaskDto>
{
    public UpdateTaskDtoValidator()
    { 
        RuleFor(t => t.Title)
            .MinimumLength(5).MaximumLength(25)
            .When(t => !string.IsNullOrEmpty(t.Title))
            .WithMessage("The title must be between 5 and 25 characters");
        
        RuleFor(t => t.Description)
            .MinimumLength(10).MaximumLength(50)
            .When(t => !string.IsNullOrEmpty(t.Description))
            .WithMessage("The description must be between 10 and 50 characters");

        RuleFor(t => t.DueDate)
            .Must(BeAValidDate)
            .When(t => t.DueDate.HasValue)
            .WithMessage("Due date cannot be in the past.");

        RuleFor(t => t.Priority)
            .IsInEnum()
            .When(t => t.Priority.HasValue)
            .WithMessage("Invalid task priority value.");
    }

    private bool BeAValidDate(DateTime? dueDate)
    {
        return dueDate!.Value.Date >= DateTime.Now.Date;
    }
}