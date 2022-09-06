using FluentValidation;
using NintendoGames.Models.DevelopersModels;

namespace NintendoGames.Models.Validators.DevelopersValidator
{
    public class DeleteDeveloperValidation : AbstractValidator<DeleteDeveloperDto>
    {
        public DeleteDeveloperValidation()
        {
            RuleFor(x => x.DeveloperId)
                .Custom((value, context) =>
                {
                    if (value is null)
                    {
                        RuleFor(x => x.DeveloperName).NotEmpty();
                    }
                });

            RuleFor(x => x.DeveloperName)
                .Custom((value, context) =>
                {
                    if (value == string.Empty)
                    {
                        RuleFor(x => x.DeveloperId).NotNull();
                    }
                });


        }
    }
}
