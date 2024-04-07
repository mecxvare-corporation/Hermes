namespace PostService.Infrastructure.Services
{
    public interface IPictureService
    {
        public Task<string> UploadImageAsync(Stream fileStream, string fileName);
        public Task DeleteImageAsync(string fileName);
        public Task<string> GetImageUrl(string fileName);
    }
}
