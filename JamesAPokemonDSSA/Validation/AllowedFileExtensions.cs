using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Validation
{
    public class AllowedFileExtensions : ValidationAttribute
    {
        private readonly string _fileExtension;
        public AllowedFileExtensions(string extension)
        {
            _fileExtension = extension;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_fileExtension.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"File extension must be PNG.";
        }
    }
}
