using Azure.Core;
using Microsoft.AspNetCore.Hosting;

namespace IK_Project.UI
{
  

        public static class FormFileExtensions
        {
            public static async Task<string> FileToStringAsync(this IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    return string.Empty; // Dosya yok veya boşsa boş bir dize dönebilirsiniz.
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] byteArray = memoryStream.ToArray();
                    return Convert.ToBase64String(byteArray);
                }
            }

            public static async Task<string> SaveFileAsync(this IFormFile file, string webRootPath, string folderName)
            {
                if (file == null || file.Length == 0)
                {
                    return string.Empty; // Dosya yok veya boşsa boş bir dize dönebilirsiniz.
                }
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
                string directoryPath = Path.Combine(webRootPath, folderName);
                string filePath = Path.Combine(directoryPath, uniqueFileName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return Path.Combine( uniqueFileName);
            }
        }
    


}
