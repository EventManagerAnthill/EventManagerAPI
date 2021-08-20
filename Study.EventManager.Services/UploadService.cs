using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System.IO;
using System.Threading.Tasks;

namespace Study.EventManager.Services
{
    public class UploadService : IUploadService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public UploadService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<FileDto> Upload(FileDto model)
        {            
            var blobContainer = _blobServiceClient.GetBlobContainerClient(model.Container);           
            var blobClient = blobContainer.GetBlobClient(model.ServerFileName + model.ImageFile.FileName);    
            
            await blobClient.UploadAsync(model.ImageFile.OpenReadStream());

            model.OriginalFileName = blobClient.Name;
            model.Url = blobClient.Uri.OriginalString;           
            return model;
        }

        public async Task<byte[]> Get(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("userfotos");

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public async Task Delete(string imageName, string container)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container);

            var blobClient = blobContainer.GetBlobClient(imageName);

            await blobClient.DeleteAsync();
        }
    }
}