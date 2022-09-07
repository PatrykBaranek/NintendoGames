using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NintendoGames.Migrations
{
    public partial class AddGameWishListEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameWishList_Game_GamesId",
                table: "GameWishList");

            migrationBuilder.DropForeignKey(
                name: "FK_GameWishList_WishList_WishListsId",
                table: "GameWishList");

            migrationBuilder.RenameColumn(
                name: "WishListsId",
                table: "GameWishList",
                newName: "WishListId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameWishList",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameWishList_WishListsId",
                table: "GameWishList",
                newName: "IX_GameWishList_WishListId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameWishList_Game_GameId",
                table: "GameWishList",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameWishList_WishList_WishListId",
                table: "GameWishList",
                column: "WishListId",
                principalTable: "WishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameWishList_Game_GameId",
                table: "GameWishList");

            migrationBuilder.DropForeignKey(
                name: "FK_GameWishList_WishList_WishListId",
                table: "GameWishList");

            migrationBuilder.RenameColumn(
                name: "WishListId",
                table: "GameWishList",
                newName: "WishListsId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameWishList",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GameWishList_WishListId",
                table: "GameWishList",
                newName: "IX_GameWishList_WishListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameWishList_Game_GamesId",
                table: "GameWishList",
                column: "GamesId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameWishList_WishList_WishListsId",
                table: "GameWishList",
                column: "WishListsId",
                principalTable: "WishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
