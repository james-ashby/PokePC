using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JamesAPokemonWAD.Migrations
{
    public partial class databasetest3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    Image = table.Column<string>(type: "varchar(255)", nullable: false),
                    ExpPerCatch = table.Column<int>(nullable: false),
                    LevelRequirement = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "CaughtPokemon",
                columns: table => new
                {
                    PokemonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PokemonName = table.Column<string>(nullable: true),
                    IsShiny = table.Column<bool>(nullable: false),
                    CatchDate = table.Column<DateTime>(nullable: false),
                    Nickname = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaughtPokemon", x => x.PokemonID);
                });

            migrationBuilder.CreateTable(
                name: "Pokemon",
                columns: table => new
                {
                    PokemonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PokemonName = table.Column<string>(type: "varchar(30)", nullable: false),
                    PokedexNum = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_Pokemon", x => x.PokemonId);
                });

            migrationBuilder.CreateTable(
                name: "AreaPokemon",
                columns: table => new
                {
                    AreaId = table.Column<int>(nullable: false),
                    PokemonId = table.Column<int>(nullable: false),
                    AreaName = table.Column<string>(nullable: true),
                    PokemonName = table.Column<string>(nullable: true),
                    Rarity = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaPokemon", x => new { x.AreaId, x.PokemonId });
                    table.ForeignKey(
                        name: "FK_AreaPokemon_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaPokemon_Pokemon_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemon",
                        principalColumn: "PokemonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaPokemon_PokemonId",
                table: "AreaPokemon",
                column: "PokemonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaPokemon");

            migrationBuilder.DropTable(
                name: "CaughtPokemon");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Pokemon");
        }
    }
}
