using FluentValidation;
using NintendoGames.Models.RatingModels;

namespace NintendoGames.Models.Validation.RatingValidator
{
    public class UpdateUserScoreValidation : AbstractValidator<UpdateUserScoreDto>
    {
        public UpdateUserScoreValidation()
        {
            RuleFor(r => r.UserScore)
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(10);

        }
    }
}
