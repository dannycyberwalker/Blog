using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
    public class ImageSaver : IImageSaver
    {

        public async Task<string> Save(IFormFile file, string path)
        {
            string fileName = Path.GetRandomFileName() + ".jpg";
            using (var fileStream = new FileStream(path + fileName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}