using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        Account account = new Account(
            "dcpokl8uw",              // Cloud name
            "768823712927488",            // API key
            "lM8sQ1W4fojgC-eBk4ccEr1XIxE" // API secret
        );

        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImageAsync(HttpPostedFileBase file)
    {
        if (file == null || file.ContentLength == 0)
            return null;

        if (file.ContentLength > 5 * 1024 * 1024)
            throw new Exception("File size is too large. Maximum size is 5MB.");

        try
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.InputStream),
                Folder = "project_uploads", // Có thể bỏ nếu lỗi hoặc test
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Debug kết quả trả về
            System.Diagnostics.Debug.WriteLine("Cloudinary upload result: " + JsonConvert.SerializeObject(uploadResult));

            if (uploadResult.Error != null)
                throw new Exception($"Cloudinary error: {uploadResult.Error.Message}");

            if (uploadResult.SecureUrl == null || string.IsNullOrEmpty(uploadResult.SecureUrl.ToString()))
                throw new Exception("Image upload failed. No URL returned from Cloudinary.");

            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while uploading the image.", ex);
        }
    }
}
