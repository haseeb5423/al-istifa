using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QnABackend.Services.Interfaces;

namespace Q_A_Backend.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");
            if (string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentException("Folder name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                throw new InvalidOperationException("WebRootPath is not set in the hosting environment.");

            var ext = Path.GetExtension(file.FileName);
            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            if (!allowedExts.Contains(ext.ToLower()))
                throw new ArgumentException("Unsupported file type.");
            if (file.Length > 5 * 1024 * 1024)
                throw new ArgumentException("File is too large (5MB max).");

            // Create target folder path: wwwroot/uploads/{folderName}
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var newFileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsFolder, newFileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Build accessible public URL
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("Unable to access current HTTP request context.");

            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/uploads/{folderName}/{newFileName}";

            return fileUrl;
        }

        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Relative path cannot be null or empty.");

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
