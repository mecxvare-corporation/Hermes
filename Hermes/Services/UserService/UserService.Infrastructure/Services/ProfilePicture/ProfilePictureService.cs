using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace UserService.Infrastructure.Services.ProfilePicture
{
    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;

        public ProfilePictureService(IConfiguration configuration)
        {
            _configuration = configuration;

            var credentials = new StorageSharedKeyCredential(_configuration["AzureStorage Key"], _configuration["AzureStorage Accaunt"]);
            var blobUri = $"https://{_configuration["AzureStorage Accaunt"]}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credentials);
            _containerClient = blobServiceClient.GetBlobContainerClient(_configuration["AzureStorage ContainerName"]);

        }
        public async Task DeleteImageAsync(string fileName)
        {

            var blob = _containerClient.GetBlobClient(fileName);

            await blob.DeleteIfExistsAsync();
        }


        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var blob = _containerClient.GetBlobClient(fileName);

            await blob.UploadAsync(fileStream);

            return fileName;
        }
    }
}
