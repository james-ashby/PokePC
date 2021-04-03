using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class removeColsAreaPokemon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaName",
                table: "AreaPokemon");

            migrationBuilder.DropColumn(
                name: "PokemonName",
                table: "AreaPokemon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaName",
                table: "AreaPokemon",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PokemonName",
                table: "AreaPokemon",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
