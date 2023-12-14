namespace UserService.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static Stream ConvertToStream(this IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0; // Reset the position to the beginning of the stream
            return memoryStream;
        }
    }
}
