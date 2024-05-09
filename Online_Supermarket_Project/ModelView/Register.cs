using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Online_Supermarket_Project.ModelView
{
    public class Register
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng Nhập Họ Tên")]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng Nhập Email")]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số Điện Thoại")]
        [Required(ErrorMessage = "Vui lòng Nhập Sđt")]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string Phone { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng Nhập Mật Khẩu")]
        [MinLength(5, ErrorMessage = "Tối thiểu 5 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Nhập lại Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật Khẩu không khớp")]
        [MinLength(5, ErrorMessage = "Tối thiểu 5 ký tự")]
        public string ConfirmPassword { get; set; }
    }
}
