using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogProjectMVC.Services
{
    public interface IImageService
    {
        string ConvertByteArrayToFile(byte[] fileData, string extension);
        Task<byte[]> ConvertFileToByteArray(IFormFile file);
        Task<byte[]> ConvertStringToByteArray(string file);
    }

     
}
