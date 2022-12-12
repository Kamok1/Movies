using Microsoft.AspNetCore.Http;

namespace Abstractions;

public interface IFileService
{
    Task SaveFile(IFormFile file, string fileName, string path);
}