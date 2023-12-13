namespace UserService.Infrastructure.Services.ProfilePicture
{
    public class ProfilePictureService : IProfilePictureService
    {
        public Task<string> UploadImageAsync(Guid userId, byte[] imageData, string imageContentType)
        {
            throw new NotImplementedException();
        }
    }
}
