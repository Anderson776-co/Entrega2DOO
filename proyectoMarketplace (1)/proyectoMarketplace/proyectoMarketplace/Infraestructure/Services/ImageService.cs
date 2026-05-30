using Application.Publications.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(Stream imageStream, string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            var folder = Path.Combine(_env.WebRootPath, "images", "publications");
            Directory.CreateDirectory(folder);

            var newFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folder, newFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await imageStream.CopyToAsync(fileStream);

            return $"/images/publications/{newFileName}";
        }
    }
}