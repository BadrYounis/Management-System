using Microsoft.AspNetCore.Http;

namespace Demo.BLL.Common.Services.Attachments
{
    public interface IAttachmentService
    {
        //Uplaod, Delete
        public Task<string?> UplaodAsync(IFormFile file, string folderName);
        public bool Delete(string filePath);
    }
}
