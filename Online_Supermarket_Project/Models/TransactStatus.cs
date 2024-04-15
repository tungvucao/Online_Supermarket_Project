using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("TransactStatus")]

    public class TransactStatus
    {
        [Key]
        public int TransactStatusId { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
