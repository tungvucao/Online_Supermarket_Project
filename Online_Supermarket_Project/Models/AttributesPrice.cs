using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Role")]
    public class AttributesPrice
    {
        [Key]
        public int AttributePriceId { get; set; }

        [Required(ErrorMessage = "AttributeId is required")]
        public int? AttributeId { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public int? Price { get; set; }

        public bool Status { get; set; }
        [ForeignKey("AttributeId")]
        public virtual Attributes? Attribute { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
