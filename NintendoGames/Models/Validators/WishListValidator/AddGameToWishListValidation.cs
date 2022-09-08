using FluentValidation;
using NintendoGames.Models.WishListModels;

namespace NintendoGames.Models.Validators.WishListValidator
{
    public class AddGameToWishListValidation : AbstractValidator<AddGameToWishListDto>
    {
        public AddGameToWishListValidation()
        {
            RuleFor(x => x.GameName)
                .NotEmpty();
        }
    }
}
