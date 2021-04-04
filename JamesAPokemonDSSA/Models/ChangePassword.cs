using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
    public class ChangePassword
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Your password requires a number, lowercase and uppercase character and a symbol.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be more than 6 characters")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
