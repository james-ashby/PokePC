using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DexClassification",
                table: "Pokedex",
                type: "varchar(60)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DexClassification",
                table: "Pokedex",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(60)");
        }
    }
}
