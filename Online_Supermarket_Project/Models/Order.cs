using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int CusId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public int? TransactStatusId { get; set; }

        public bool? Deleted { get; set; }

        public bool? Paid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int? PaymentId { get; set; }

        public string? Note { get; set; }
        [ForeignKey("CusId")]
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [ForeignKey("TransactStatusId")]

        public virtual TransactStatus? TransactStatus { get; set; }
    }
}
