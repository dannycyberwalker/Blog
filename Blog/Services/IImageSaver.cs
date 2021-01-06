using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
    
    public interface IImageSaver
    {
        /// <summary>
        /// Method for save file.
        /// </summary>
        /// <param name="file">Form file from input.</param>
        /// /// <param name="path">Path where file will save.</param>
        /// <returns>File name.</returns>
        Task<string> Save(IFormFile file, string path);
    }
}