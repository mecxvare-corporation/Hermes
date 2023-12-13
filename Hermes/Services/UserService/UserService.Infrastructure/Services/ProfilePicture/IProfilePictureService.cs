namespace UserService.Infrastructure.Services.ProfilePicture
{
    public interface IProfilePictureService
    {
        public Task<string> UploadImageAsync(Guid userId, byte[] imageData, string imageContentType);
    }
}
