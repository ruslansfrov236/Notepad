using Microsoft.AspNetCore.Http;
using notepad.business.Abstract;

namespace notepad.business.Concrete;

public class FileService:IFileService
{
    
    public bool IsVideo(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (!file.ContentType.StartsWith("video/"))
            return false;

        var allowedExtensions = new[] { ".mp4" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(fileExtension);
    }

    public bool CheckSize(IFormFile file, int maxSize)
        => (file.Length / 1024) > maxSize;

    public void Delete(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public async Task<string> UploadVideoAsync(IFormFile file)
    {
        var filename = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}.png";
        var tempPath = Path.Combine(Directory.GetCurrentDirectory() ,"wwwroot/assets/video/" + file.FileName);
     
           
        using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
        {
            await file.CopyToAsync(fileStream);
        }

        return filename;
    }

    public bool IsImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (!file.ContentType.StartsWith("image/"))
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(fileExtension);
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var filename = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}.png";
        var tempPath = Path.Combine( Directory.GetCurrentDirectory(),  "wwwroot/assets/image/" + file.FileName);
     
           
        using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
        {
            await file.CopyToAsync(fileStream);
        }
        return filename;
    }

}