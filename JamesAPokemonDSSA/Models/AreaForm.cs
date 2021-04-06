using JamesAPokemonWAD.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class AreaForm
    {
        public AreaForm()
        {
            AllPoke = new List<Pokemon>();
            PokemonIds = new List<int>();
        }
        [Key]
        public int AreaId { get; set; }
        [Required]
        public string AreaName { get; set; }
        [AllowedFileExtensions(".png")]
        [MaxFileSize(1000 * 500)]
        public IFormFile UploadImage { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int ExpPerCatch { get; set; }
        [Required]
        public int LevelRequirement { get; set; }
        public List<Pokemon> AllPoke { get; set; }
        [Required(ErrorMessage = "You must select at least 1 Pokémon")]
        public List<int> PokemonIds { get; set; }

    }
}
