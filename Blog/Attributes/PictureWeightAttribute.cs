using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Blog.Attributes
{
    public class PictureWeightAttribute : ValidationAttribute
    {
        private long LengthInBytes { get;}
        public PictureWeightAttribute(long lengthInBytes)
        {
            LengthInBytes = lengthInBytes;
        }
        private string GetErrorMessage(long fileLengthInBytes) =>
            $"The weight of your file is {fileLengthInBytes / 1024 / 1024} MB , and it must be less than {LengthInBytes / 1024 / 1024} MB.";


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
                if (file.Length > LengthInBytes)
                    return new ValidationResult(GetErrorMessage(file.Length));
            
            return ValidationResult.Success;
        }
    }
}