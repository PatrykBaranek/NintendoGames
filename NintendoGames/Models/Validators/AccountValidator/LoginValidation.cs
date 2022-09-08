using FluentValidation;
using NintendoGames.Models.AccountModels;

namespace NintendoGames.Models.Validators.AccountValidator
{
    public class LoginValidation : AbstractValidator<LoginDto>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();

        }
    }
}
