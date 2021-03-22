using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class addmigrationinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DexType_2",
                table: "Pokedex",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DexType_1",
                table: "Pokedex",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DexType_2",
                table: "Pokedex",
                type: "varchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DexType_1",
                table: "Pokedex",
                type: "varchar(15)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }
    }
}
