using FluentValidation;
using NintendoGames.Models.DevelopersModels;

namespace NintendoGames.Models.Validation.DevelopersValidator
{
    public class DeleteDeveloperValidation : AbstractValidator<DeleteDeveloperDto>
    {
        public DeleteDeveloperValidation()
        {

        }
    }
}
