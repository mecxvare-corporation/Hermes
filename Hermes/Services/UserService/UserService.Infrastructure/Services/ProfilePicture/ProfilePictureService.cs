using Azure.Storage;
using Azure.Storage.Blobs;

namespace UserService.Infrastructure.Services.ProfilePicture
{
    public class ProfilePictureService : IProfilePictureService
    {
        private readonly string _storageAcc = string.Empty;
        private readonly string _key = string.Empty;
        private readonly string _containerName = string.Empty;
        private readonly BlobContainerClient _containerClient;

        public ProfilePictureService()
        {
            var credentials = new StorageSharedKeyCredential(_key, _storageAcc);
            var blobUri = $"https://{_storageAcc}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credentials);
            _containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

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
