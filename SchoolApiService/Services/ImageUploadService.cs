using SchoolApp.Models.DataModels.StaticModel;

namespace SchoolApiService.Services
{
    public class ImageUploadService
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public ImageUploadService(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }


        public async Task<string?> Upload(ImageUpload upload)
        {
            try
            {
                if (string.IsNullOrEmpty(upload.ImageData)) return null;

                string base64Data = upload.ImageData;
                
                // Strip data URI prefix if present (e.g., "data:image/jpeg;base64,")
                if (base64Data.Contains(";base64,"))
                {
                    base64Data = base64Data.Split(";base64,")[1];
                }

                // Handle Base64URL characters if any
                base64Data = base64Data.Replace('_', '/').Replace('-', '+');
                
                // Add padding if missing
                int mod4 = base64Data.Length % 4;
                if (mod4 > 0) base64Data += new string('=', 4 - mod4);

                byte[] bits = Convert.FromBase64String(base64Data);

                if (string.IsNullOrEmpty(upload.ImageName))
                {
                    upload.ImageName = Guid.NewGuid().ToString() + ".png";
                }

                // Ensure the images directory exists
                string imagesDir = Path.Combine(hostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(imagesDir))
                {
                    Directory.CreateDirectory(imagesDir);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.ImageName);
                if (string.IsNullOrEmpty(Path.GetExtension(fileName))) fileName += ".png";
                
                string relativePath = "/images/" + fileName;
                string fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);

                await File.WriteAllBytesAsync(fullPath, bits);

                return relativePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }

        public void DeleteOldImage(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath) || !relativePath.StartsWith("/images/")) return;

            try
            {
                string fullPath = Path.Combine(hostEnvironment.WebRootPath, relativePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting old image: {ex.Message}");
            }
        }


    }
}
