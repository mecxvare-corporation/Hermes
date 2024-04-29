using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

using Microsoft.Extensions.Configuration;

namespace PostService.Infrastructure.Services
{
    public class PictureService : IPictureService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;

        public PictureService(IConfiguration configuration)
        {
            _configuration = configuration;

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

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/jpeg" }
            };

            await blob.UploadAsync(fileStream, options);

            return fileName;
        }

        public async Task<string> GetImageUrl(string fileName)
        {
            if (fileName == string.Empty || fileName == null)
            {
                return string.Empty;
            }
            var blob = _containerClient.GetBlobClient(fileName);

            var blobSasUri = await CreateServiceSASBlob(blob);

            // Create a blob client object representing 'image' with SAS authorization
            var blobClientSAS = new BlobClient(blobSasUri);

            return blobClientSAS.Uri.ToString();
        }

        private async Task<Uri> CreateServiceSASBlob(BlobClient blobClient, string storedPolicyName = null)
        {
            // Check if BlobContainerClient object has been authorized with Shared Key
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one day
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    ContentDisposition = "inline"

                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasURI = blobClient.GenerateSasUri(sasBuilder);

                return sasURI;
            }
            else
            {
                // Client object is not authorized via Shared Key
                return null;
            }
        }

    }
}
