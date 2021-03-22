using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class dexnumupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PokedexNum",
                table: "Pokemon",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PokedexNum",
                table: "Pokemon");
        }
    }
}
