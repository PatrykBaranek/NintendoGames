using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NintendoGames.Migrations
{
    public partial class AddedOnAddToGameWishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "GameWishList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "GameWishList");
        }
    }
}
