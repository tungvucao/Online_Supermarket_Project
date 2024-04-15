using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? OrderNumber { get; set; }

        public int? Quantity { get; set; }

        public int? Discount { get; set; }

        public int? Total { get; set; }

        public DateTime? ShipDate { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}
