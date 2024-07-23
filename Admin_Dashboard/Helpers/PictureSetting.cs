using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Admin_Dashboard.Helpers
{
    public class PictureSetting
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is invalid.");
            }

            // 1. Get Folder Path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);
            Directory.CreateDirectory(folderPath); // Ensure the folder exists

            // 2. Set Unique FileName
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // 3. Get File Path
            var filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Streams
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            // 5. Return FileName
            return Path.Combine("images", folderName, fileName).Replace("\\", "/");
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
