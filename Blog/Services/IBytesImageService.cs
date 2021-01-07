using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
    public interface IBytesImageService
    {
        byte[] GetBytesFrom(IFormFile image);
    }
}