using JamesAPokemonWAD.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class AddPokemon
    {
        public AddPokemon()
        {
            SelectedAreas = new List<Area>();
        }
        [Key]
        public int PokemonId { get; set; }
        [Required]
        public string PokemonName { get; set; }

        [Required]
        public int PokedexNum { get; set; }

        [Required]
        public string Type_1 { get; set; }
        public string Type_2 { get; set; } // Not required since some Pokémon do not have a secondary type

        [Required]
        public string Classification { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Height { get; set; }      // Height in metres

        [Required]
        public decimal Weight { get; set; }      // Weight in kilograms

        [Required]
        public decimal Generation { get; set; }
        [Required(ErrorMessage = "Please upload the Pokémon Image")]
        [AllowedFileExtensions(".png")]
        [MaxFileSize(1000 * 500)]
        public IFormFile UploadImage { get; set; }

        [Required(ErrorMessage = "Please upload the Shiny Pokémon Image")]
        [AllowedFileExtensions(".png")]
        [MaxFileSize(1000 * 500)]
        public IFormFile UploadShinyImage { get; set; }
        public string ImageUrl { get; set; }
        public string ShinyImageUrl { get; set; }
        public string Rarity { get; set; }
        public List<Area> SelectedAreas { get; set; }
        public List<int> AreaIds { get; set; }
    }
}
