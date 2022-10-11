using System.ComponentModel.DataAnnotations;

namespace Microservice.Accounting.Models.DTOs
{
    public class LoginViewModel
    {
        [MaxLength(15)]
        [MinLength(11, ErrorMessage =
            "اندازه {0} از حد مجاز {1} کاراکتر کمتر است")]
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Phone { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RemmberMe { get; set; }
    }
}