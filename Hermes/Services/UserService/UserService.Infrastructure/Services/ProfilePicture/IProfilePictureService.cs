namespace UserService.Infrastructure.Services.ProfilePicture
{
    public interface IProfilePictureService
    {
        public Task<string> UploadImageAsync(Stream fileStream, string fileName);
        public Task DeleteImageAsync(string fileName);
        public Task<string> GetImageUrl(string fileName);

    }
}
