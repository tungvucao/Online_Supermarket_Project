using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Online_Supermarket_Project.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }

        public string? Salt { get; set; }

        public bool Status { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }

        public int RoleId { get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("RoleId")]
        public virtual Roles? Role { get; set; }
    }
}