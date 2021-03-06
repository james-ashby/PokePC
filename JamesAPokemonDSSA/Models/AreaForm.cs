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
        [Required(ErrorMessage ="Please enter an Area name.")]
        public string AreaName { get; set; }
        [AllowedFileExtensions(".png")]
        [MaxFileSize(1000 * 750)]
        public IFormFile UploadImage { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage ="Please enter the experience gained per catch.")]
        public int ExpPerCatch { get; set; }
        [Required(ErrorMessage ="Please enter the area's level requirement.")]
        public int LevelRequirement { get; set; }
        public List<Pokemon> AllPoke { get; set; }
        [Required(ErrorMessage = "You must select at least 1 Pokémon")]
        public List<int> PokemonIds { get; set; }

    }
}
