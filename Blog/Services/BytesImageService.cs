using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
    public class BytesImageService : IBytesImageService
    {
        
        /// <summary>
        /// Help to convert image to array of bytes.
        /// </summary>
        /// <param name="image">Image file from view.</param>
        /// <returns>Picture in byte representation.</returns>
        /// <exception cref="NullReferenceException">throw if input file equal null.</exception>
        public byte[] GetBytesFrom(IFormFile image)
        {
            byte[] imageData = null;
            if (image != null)
                using (var binaryReader = new BinaryReader(image.OpenReadStream()))
                    imageData = binaryReader.ReadBytes((int)image.Length);
            else
                throw new NullReferenceException("Input object is null");
            
            return imageData;
        }
    }
}