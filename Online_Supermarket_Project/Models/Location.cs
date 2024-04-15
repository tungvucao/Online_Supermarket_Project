using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Location")]
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Slug { get; set; }

        public string? NameWithType { get; set; }

        public string? PathWithType { get; set; }

        public int? ParentCode { get; set; }

        public int? Level { get; set; }

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
