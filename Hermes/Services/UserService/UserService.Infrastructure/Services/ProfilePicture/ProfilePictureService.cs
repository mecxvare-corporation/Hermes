namespace UserService.Infrastructure.Services.ProfilePicture
{
    public class ProfilePictureService : IProfilePictureService
    {
        public Task DeleteImageAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadImageAsync(Guid userId, byte[] imageData, string fileName, string fileExtension)
        {
            throw new NotImplementedException();
        }
    }
}
