using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "ProductName is required")]
        [MaxLength(100)]
        public string ProductName { get; set; } = null!;

        public string? ShortDesc { get; set; }

        public string? Description { get; set; }

        public int CateId { get; set; }

        public int? Price { get; set; }

        public int? Discount { get; set; }

        public string? Image { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Status { get; set; }

        public string? Title { get; set; }

        public string? Alias { get; set; }

        public int? UnitsInStock { get; set; }

        public virtual ICollection<AttributesPrice> AttributesPrices { get; set; } = new List<AttributesPrice>();
        [ForeignKey("CateId")]
        public virtual Category? Cate { get; set; }
    }
}
