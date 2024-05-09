using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Online_Supermarket_Project.ModelView
{
    public class ChangePasswordVM
    {
        [Key]
        public int CusId { get; set; }
        [Display(Name = "Current Password")]
        public string PasswordNow { get; set; }
        [Display(Name = "New Password")]
        [Required(ErrorMessage ="Password is required")]
        [MinLength(5,ErrorMessage ="Min 5 charactor")]
        public string Password { get; set; }
        [Display(Name = "Password Confirm")]
        [MinLength(5, ErrorMessage = "Min 5 charactor")]
        [Compare("Password", ErrorMessage = "Passwoord invalid!")]
        public string ConfirmPassword { get; set; }
    }
}
