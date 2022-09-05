using FluentValidation;
using NintendoGames.Models.DevelopersModels;

namespace NintendoGames.Models.Validation.DevelopersValidator
{
    public class AddDeveloperValidation : AbstractValidator<AddDeveloperDto>
    {
        public AddDeveloperValidation()
        {
            RuleFor(d => d.DeveloperName)
                .NotEmpty();

        }
    }
}
