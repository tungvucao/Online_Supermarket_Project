using System.ComponentModel.DataAnnotations;

namespace Online_Supermarket_Project.ModelView
{
    public class LoginViewModel
    {
        [Key]
        [Required(ErrorMessage = " Vui lòng Nhập SĐT hoặc Email")]
        [Display(Name = "SĐT / Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserName { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng Nhập mật khẩu")]
        public string Password { get; set; }
    }
}
