using FluentValidation;
using NintendoGames.Models.GenresModels;

namespace NintendoGames.Models.Validators.GenreValidator
{
    public class AddGenreValidation : AbstractValidator<AddGenreDto>
    {
        public AddGenreValidation()
        {
            RuleFor(g => g.GenreName)
                .NotEmpty();
        }
    }
}
