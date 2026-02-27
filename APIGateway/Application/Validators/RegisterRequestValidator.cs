using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class RegisterRequestValidator: AbstractValidator<RegisterDTO>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(20);

            RuleFor(x => x.LastName).NotEmpty().MaximumLength(20);

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MinimumLength(6).MaximumLength(100);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
