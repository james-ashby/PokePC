using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class Register
    {
        [Required]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Your password requires a number, lowercase and uppercase character and a symbol.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be more than 6 characters")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Starter { get; set; }

    }
}
