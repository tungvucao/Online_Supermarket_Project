using System.ComponentModel.DataAnnotations;

namespace Online_Supermarket_Project.ModelView
{
    public class MuaHangVM
    {
        public int CusId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ nhận hàng")]
        public string Address { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập SĐT")]

        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        public string FullName { get; set; }
        public string Note { get; set; }
    }
}
