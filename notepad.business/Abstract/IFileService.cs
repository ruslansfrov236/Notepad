using Microsoft.AspNetCore.Http;

namespace notepad.business.Abstract;

public interface IFileService
{
    
    Task<string> UploadAsync(IFormFile file);
    Task<string> UploadVideoAsync(IFormFile file);

    bool IsImage(IFormFile file);
    bool IsVideo(IFormFile file);
    bool CheckSize(IFormFile file, int maxSize);
    void Delete(string path);
}