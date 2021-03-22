using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pokemon",
                columns: table => new
                {
                    PokemonName = table.Column<string>(type: "varchar(30)", nullable: false),
                    Type_1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    Type_2 = table.Column<string>(type: "varchar(50)", nullable: true),
                    Classification = table.Column<string>(type: "varchar(60)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Generation = table.Column<decimal>(type: "decimal(10)", nullable: false),
                    Image = table.Column<string>(type: "varchar(255)", nullable: false),
                    ShinyImage = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemon", x => x.PokemonName);
                });

            migrationBuilder.CreateTable(
                name: "CaughtPokemon",
                columns: table => new
                {
                    PokemonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    PokemonName = table.Column<string>(nullable: true),
                    IsShiny = table.Column<bool>(nullable: false),
                    CatchDate = table.Column<DateTime>(nullable: false),
                    Nickname = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaughtPokemon", x => x.PokemonID);
                    table.ForeignKey(
                        name: "FK_CaughtPokemon_Pokemon_PokemonName",
                        column: x => x.PokemonName,
                        principalTable: "Pokemon",
                        principalColumn: "PokemonName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaughtPokemon_PokemonName",
                table: "CaughtPokemon",
                column: "PokemonName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaughtPokemon");

            migrationBuilder.DropTable(
                name: "Pokemon");
        }
    }
}
