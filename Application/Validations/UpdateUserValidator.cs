
using Application.Dto.UserDto;
using FluentValidation;

namespace Application.Validations;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email address format.");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.FirstName)
            .MinimumLength(4)
            .When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage("First name must be at least 4 characters.");

        RuleFor(x => x.LastName)
            .MinimumLength(4)
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage("Last name must be at least 4 characters.");
    }
}