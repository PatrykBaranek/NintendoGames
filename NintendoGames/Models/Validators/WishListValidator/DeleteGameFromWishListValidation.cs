using FluentValidation;
using NintendoGames.Entities;
using NintendoGames.Models.WishListModels;

namespace NintendoGames.Models.Validators.WishListValidator
{
    public class DeleteGameFromWishListValidation : AbstractValidator<DeleteGameFromWishListDto>
    {
        public DeleteGameFromWishListValidation(NintendoDbContext dbContext)
        {
            RuleFor(x => x.GameName)
                .NotEmpty();
        }
    }
}
