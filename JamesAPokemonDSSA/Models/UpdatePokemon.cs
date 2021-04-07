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
    public class UpdatePokemon
    {
        public UpdatePokemon()
        {
            SelectedAreas = new List<Area>();
        }
        [Key]
        public int PokemonId { get; set; }
        [Required(ErrorMessage = "Please enter a Pokémon name.")]
        public string PokemonName { get; set; }

        [Required(ErrorMessage = "Please enter a Pokédex number.")]
        public int PokedexNum { get; set; }

        [Required]
        public string Type_1 { get; set; }
        public string Type_2 { get; set; } // Not required since some Pokémon do not have a secondary type

        [Required(ErrorMessage = "Please enter a classification/species, i.e 'Mouse Pokémon'")]
        public string Classification { get; set; }

        [Required(ErrorMessage = "Please enter a description of the Pokémon.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a height.")]
        public decimal Height { get; set; }      // Height in metres

        [Required(ErrorMessage = "Please enter a weight.")]
        public decimal Weight { get; set; }      // Weight in kilograms

        [Required(ErrorMessage = "Please enter a generation number.")]
        public decimal Generation { get; set; }

        [AllowedFileExtensions(".png, .gif")]
        [MaxFileSize(1000 * 500)]
        public IFormFile UploadImage { get; set; }
        [AllowedFileExtensions(".png, .gif")]
        [MaxFileSize(1000 * 500)]
        public IFormFile UploadShinyImage { get; set; }
        public string ImageUrl { get; set; }
        public string Rarity { get; set; }
        public List<Area> SelectedAreas { get; set; }
        public List<int> AreaIds { get; set; }
    }
}
