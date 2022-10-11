using System.ComponentModel.DataAnnotations;

namespace Microservice.Seles.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Serial { get; set; }

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(15)]
        [MinLength(11,ErrorMessage ="اندازه {0} از حد مجاز {1} کاراکتر کمتر است")]
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Phone { get; set; }

        [MaxLength(15)]
        [Display(Name = "نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Role { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

    }
}
