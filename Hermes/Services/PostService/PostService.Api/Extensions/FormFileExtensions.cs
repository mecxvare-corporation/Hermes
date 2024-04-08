namespace PostService.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static Stream ConvertToStream(this IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0; 
            return memoryStream;
        }
    }
}
