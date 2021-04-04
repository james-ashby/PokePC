using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JamesAPokemonWAD.Models
{
        public class MaxFileSize : ValidationAttribute
        {
            private readonly int _maxFileSize;
            public MaxFileSize(int maxFileSize)
            {
                _maxFileSize = maxFileSize;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var image = value as IFormFile;
                if (image != null)
                {
                    if (image.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"The maximum file size is { _maxFileSize / 100} kilobytes.";
            }
        }
}
