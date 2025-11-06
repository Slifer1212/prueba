using System.Data;
using Application.Dto.UserDto;
using FluentValidation;

namespace Application.Validations;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("The email address is required.");
        RuleFor(x => x.Password).NotEmpty().NotNull()
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.FirstName).NotEmpty().NotNull().MinimumLength(4);
        RuleFor(x => x.LastName).NotEmpty().NotNull().MinimumLength(4);
        
    }
}