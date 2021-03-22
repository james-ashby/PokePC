using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class imageurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DexImage",
                table: "Pokedex",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DexShinyImage",
                table: "Pokedex",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DexImage",
                table: "Pokedex");

            migrationBuilder.DropColumn(
                name: "DexShinyImage",
                table: "Pokedex");
        }
    }
}
