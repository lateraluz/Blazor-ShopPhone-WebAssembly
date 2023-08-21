using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Interfaces;

public interface IFileUploader
{
    Task<string> UploadFileAsync(string? base64String, string? fileName);
}