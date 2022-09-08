using FluentValidation;
using NintendoGames.Entities;
using NintendoGames.Models.AccountModels;

namespace NintendoGames.Models.Validators.AccountValidator
{
    public class CreateUserValidation : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidation(NintendoDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty()
                .Custom((value, context) =>
                {
                    if (dbContext.User.Any(u => u.Email == value))
                    {
                        context.AddFailure("Email already exist");
                    }
                });

            RuleFor(x => x.Password)
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);
        }
    }
}
