using Microsoft.AspNetCore.Http;

namespace Demo.BLL.Common.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        //Allowed extensions [.png, .jpg, .jpeg]
        public readonly List<string> _allowedExtensions = new() { ".png", ".jpg", ".jpeg" };
        //Max Size   2MB
        public const int _maxAllowedSize = 2_097_152;
        public async Task<string?> UplaodAsync(IFormFile file, string folderName)
        {
            //1] Validate for extensions [".png", ". jpg", ". jpeg"]
            var extension = Path.GetExtension(file.FileName); //Mariam.png
            if (!_allowedExtensions.Contains(extension))
                return null;
            //2] Validate for Max size[2_097_152; //2MB]
            if (file.Length > _maxAllowedSize)
                return null;
            //3] Get located folder path
            //var folderPath = "D:\\Route\\MVC\\MVCApp\\Demo.PL\\wwwroot\\files\\images\\";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            //4] Set unique file name
            //13291217281.png
            var fileName = $"{Guid.NewGuid()} {extension}";
            //5] Get file path [FolderPath + FileName]
            var filePath = Path.Combine(folderPath, fileName);
            //6] Save file as stream[Data per time]
            using var fileStream = new FileStream(filePath, FileMode.Create);
            //7] Copy file to the stream
            await file.CopyToAsync(fileStream);
            //8]I Return file name
            return fileName;
            /*
                * File streaming :
                1]You Open the Image File
                FileStream establishes a connection between your program and the image file
                The file is opened in read, write, or both modes.
                
                2]You Read or Write Binary Data
                Images are stored as bytes (Os and 1s), not as human-readable text.
                
                3]You Close the File After Use
                Keeping an image file open too long can cause file locks or memory issues.
                Use using to ensure proper closure.
            */
            /*
                FileMode.Create Creates a new file.Overwrites if the file already exists.
                FileMode. Open  Opens an existing file. Throws an exception if it doesn't exists
                FileMode. Append Opens the file if it exists or creates a new one. Data s written to the end of the file
                FileMode. Truncate Opens an existing file and clears its content.
                File Mode. OpenOrCreate Opens the file if it exists or creates new one.
            */
        }
        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}
