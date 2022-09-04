using FluentValidation;
using NintendoGames.Models.RatingModels;

namespace NintendoGames.Models.Validation
{
    public class RatingValidation : AbstractValidator<UpdateUserScoreDto>
    {
        public RatingValidation()
        {
            RuleFor(r => r.UserScore)
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(10);

        }
    }
}
