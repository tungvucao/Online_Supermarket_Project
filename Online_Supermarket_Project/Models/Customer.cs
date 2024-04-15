using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CusId { get; set; }

        public string? FullName { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Avatar { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int? LocationId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool Status { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
