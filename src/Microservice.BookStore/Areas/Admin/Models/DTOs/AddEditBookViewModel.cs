using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Microservice.BookStore.Models.DTOs
{
    public class AddEditBookViewModel
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "عنوان کتاب")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "توضیحات کتاب")]
        public string Description { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "نویسنده")]
        public string Auther { get; set; }

        [Required]
        [Display(Name = "قیمت")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "تعداد")]
        public int Count { get; set; }

        [Display(Name = "تصویر")]
        public IFormFile Image { get; set; }

        public string OldImage { get; set; }
    }
}