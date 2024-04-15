using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Attributes")]
    public class Attributes
    {
        [Key]
        public int AttributeId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<AttributesPrice> AttributesPrices { get; set; } = new List<AttributesPrice>();
    }
}
