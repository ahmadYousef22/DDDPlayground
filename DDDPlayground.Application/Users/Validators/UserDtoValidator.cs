using DDDPlayground.Domain.Entities;
using FluentValidation;

namespace DDDPlayground.Application.Users.Validators
{
    public class UserDtoValidator : AbstractValidator<User>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
