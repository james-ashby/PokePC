using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Please enter a username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Your password must contain at least 1 number, a lowercase and an uppercase character and a symbol.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be more than 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress]
        public string Email { get; set; }
        public string Starter { get; set; }

    }
}
