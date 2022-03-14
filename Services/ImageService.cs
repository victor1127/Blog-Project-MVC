using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Services
{
    public class ImageService : IImageService
    {
        public string ConvertByteArrayToFile(byte[] fileData, string extension)
        {
            if (fileData is null || extension is null) return null;

            var imageData = Convert.ToBase64String(fileData);
            return $"data:{extension};base64,{imageData}";
        }

        public async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            if (file is null) return null;

            using MemoryStream stream = new();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
    }

     
}
