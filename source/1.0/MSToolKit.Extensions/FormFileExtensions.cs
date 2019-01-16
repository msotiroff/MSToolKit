using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MSToolKit.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsImage(this IFormFile formFile)
        {
            var hasCorrectContentType = formFile.ContentType.Contains("image");

            if (!hasCorrectContentType)
            {
                return false;
            }

            var hasCorrectExtension = false;
            var fileExtension = formFile.FileName.Split('.').Last();
            switch (fileExtension.ToLower())
            {
                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                case "gif":
                    hasCorrectExtension = true;
                    break;
                default:
                    hasCorrectExtension = false;
                    break;
            }

            if (!hasCorrectExtension)
            {
                return false;
            }
            
            return true;
        }
    }
}
