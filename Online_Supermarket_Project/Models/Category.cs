using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CateId { get; set; }
        [MaxLength(100)]
        public string? CateName { get; set; }

        public string? Desciption { get; set; }
        public bool Status { get; set; }
        [MaxLength(250)]
        public string? Title { get; set; }
        [MaxLength(250)]
        public string? Alias { get; set; }
        [MaxLength(250)]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
