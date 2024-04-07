using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Infrastructure.Services
{
    public class PictureService : IPictureService
    {
        public Task DeleteImageAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetImageUrl(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
