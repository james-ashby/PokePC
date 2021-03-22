﻿// <auto-generated />
using System;
using JamesAPokemonWAD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JamesAPokemonWAD.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210321135131_update3")]
    partial class update3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JamesAPokemonWAD.Models.DexEntry", b =>
                {
                    b.Property<int>("DexID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DexClassification")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.Property<string>("DexDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("DexGeneration")
                        .HasColumnType("decimal(10)");

                    b.Property<decimal>("DexHeight")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("DexName")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<string>("DexType_1")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DexType_2")
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("DexWeight")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("DexID");

                    b.ToTable("Pokedex");
                });

            modelBuilder.Entity("JamesAPokemonWAD.Models.Pokemon", b =>
                {
                    b.Property<int>("PokeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CatchDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsShiny")
                        .HasColumnType("bit");

                    b.Property<string>("Nickname")
                        .HasColumnType("varchar(15)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PokeID");

                    b.ToTable("AllPokemon");
                });
#pragma warning restore 612, 618
        }
    }
}
