using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllPokemon",
                columns: table => new
                {
                    PokeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    IsShiny = table.Column<bool>(nullable: false),
                    CatchDate = table.Column<DateTime>(nullable: false),
                    Nickname = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPokemon", x => x.PokeID);
                });

            migrationBuilder.CreateTable(
                name: "Pokedex",
                columns: table => new
                {
                    DexID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DexName = table.Column<string>(type: "varchar(30)", nullable: false),
                    DexType_1 = table.Column<string>(type: "varchar(15)", nullable: false),
                    DexType_2 = table.Column<string>(type: "varchar(15)", nullable: true),
                    DexClassification = table.Column<string>(type: "varchar(50)", nullable: false),
                    DexDescription = table.Column<string>(type: "text", nullable: false),
                    DexHeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DexWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DexGeneration = table.Column<decimal>(type: "decimal(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokedex", x => x.DexID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllPokemon");

            migrationBuilder.DropTable(
                name: "Pokedex");
        }
    }
}
