using Abstractions;
using Data;
using Microsoft.AspNetCore.Http;
using Models.Settings;

namespace Implementations;

public class FileService : IFileService
{
    public async Task SaveFile(IFormFile file, string fileName, string path)
    {
        EnsureThatPathExists(path);
        await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }
    public static void EnsureThatPathExists(string path)
    {
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
    }
}