namespace UserService.Infrastructure.Services.ProfilePicture
{
    public interface IProfilePictureService
    {
        public Task<string> UploadImageAsync(Guid userId, byte[] imageData, string fileName, string fileExtension);
        public Task DeleteImageAsync(Guid userId);
    }
}
