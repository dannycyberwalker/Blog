using Microsoft.AspNetCore.Http;

namespace Blog.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Help to convert image to array of bytes.
        /// </summary>
        /// <param name="image">Image file from view.</param>
        /// <returns>Picture in byte representation.</returns>
        /// <exception cref="NullReferenceException">throw if input file equal null.</exception>
        byte[] GetBytesFrom(IFormFile image);
        /// <summary>
        /// Helps to check if a file is an image.
        /// </summary>
        /// <param name="image">Some file for check</param>
        /// <returns>File extention, like ".(extention)" </returns>
        bool IsImage(IFormFile image);
    }
}