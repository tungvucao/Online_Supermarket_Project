using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        [MaxLength(50)] // Đặt độ dài tối đa cho RoleName
        public string? RoleName { get; set; }

        public string? Description { get; set; }

        // Mối quan hệ 1-n với bảng Account
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
