using Abstractions;
using Microsoft.AspNetCore.Http;

namespace Implementations;

public class FileService : IFileService
{
    public async Task SaveFile(IFormFile file, string fileName, string path)
    {
        Directory.CreateDirectory(path);
        await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }
}