using FluentValidation;
using NintendoGames.Models.UserModels;

namespace NintendoGames.Models.Validators.UserValidator
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
