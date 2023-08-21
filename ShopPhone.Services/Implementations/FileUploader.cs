using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public class FileUploader : IFileUploader
{
    private readonly AppConfig _appConfig;
    private readonly ILogger<FileUploader> _logger;

    public FileUploader(IOptions<AppConfig> options, ILogger<FileUploader> logger)
    {
        _appConfig = options.Value;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(string? base64String, string? fileName)
    {
        if (string.IsNullOrWhiteSpace(base64String) || string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        try
        {
            var bytes = Convert.FromBase64String(base64String);

            // C:\MitoCode\Imagenes\Adele.jpg (Windows) Backslash (\)
            // ~/Imagenes/Adele.jpg` (Linux) Forward slash (/)

            var path = Path.Combine(_appConfig.StorageConfiguration.Path, fileName);

            await using var fileStream = new FileStream(path, FileMode.Create);
            await fileStream.WriteAsync(bytes, 0, bytes.Length);

            // http://localhost:8080/imagenes/adele.jpg

            return $"{_appConfig.StorageConfiguration.PublicUrl}{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al subir el archivo {fileName} {message}", fileName, ex.Message);
            return string.Empty;
        }
    }
}