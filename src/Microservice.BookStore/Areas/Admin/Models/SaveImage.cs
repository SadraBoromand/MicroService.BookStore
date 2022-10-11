using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Microservice.BookStore.Models
{
    public class SaveImage
    {
        public string Save(IFormFile image)
        {
            if (image != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                path = Path.Combine(path, imageName);
                using (var fileStram = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(fileStram);
                }

                return Path.Combine("Images", imageName).ToString();
            }
            else
            {
                return Path.Combine("Images", "Default.png").ToString();
            }
        }

        public bool Delete(string imagePath)
        {
            if (imagePath != Path.Combine("Images", "Default.png"))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    imagePath);
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}