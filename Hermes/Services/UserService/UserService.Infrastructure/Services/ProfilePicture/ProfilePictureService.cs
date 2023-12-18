using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace UserService.Infrastructure.Services.ProfilePicture
{
    [ExcludeFromCodeCoverage]
    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;

        public ProfilePictureService()
        {
            _configuration = new ConfigurationBuilder()
            .AddUserSecrets<ProfilePictureService>()
            .Build();

            var credentials = new StorageSharedKeyCredential(_configuration["AzureStorageAccount"], _configuration["AzureStorageKey"]);
            var blobUri = $"https://{_configuration["AzureStorageAccount"]}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credentials);
            _containerClient = blobServiceClient.GetBlobContainerClient(_configuration["AzureStorageContainerName"]);

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

        public string GetImageUrl(string fileName)
        {
            var blob = _containerClient.GetBlobClient(fileName);

            return blob.Uri.ToString();
        }
    }
}
