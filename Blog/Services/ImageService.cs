using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
    public class ImageService : IImageService
    {
        
       
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

        public bool IsImage(IFormFile image)
        {
            List<List<string>> extentions = new List<List<string>>()
            {
                "89 50 4E 47".Split().ToList(),
                "FF D8 FF DB".Split().ToList(),
                "FF D8 FF E0".Split().ToList()
            };

            var file = GetBytesFrom(image);
            List<string> fileHead = new List<string>();
            for (var i = 0; i < 4; i++)
                fileHead.Add(file[i].ToString("X2"));


            foreach (var extentionHead in extentions)
                if (!extentionHead.Except(fileHead).Any())
                    return true;

            return false;
        }
    }
}