using Azure.Storage.Blobs;
using Study.EventManager.Services.Dto;
using System.IO;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface IUploadService
    {
        Task<FileDto> Upload(FileDto model);
        Task<byte[]> Get(string imageName);
        Task Delete(string imageName, string container);
    }
}
