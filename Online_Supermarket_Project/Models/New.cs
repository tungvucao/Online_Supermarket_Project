using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("New")]
    public class New
    {
        [Key]
        public int NewId { get; set; }

        public string Title { get; set; }

        public string? ShortContent { get; set; }

        public string? Content { get; set; }

        public string? Thumb { get; set; }

        public bool Published { get; set; }

        public string? Alias { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Author { get; set; }

        public int AccountId { get; set; }

        public string? Tags { get; set; }

        public int CateId { get; set; }

        public bool IsHot { get; set; }

        public bool IsNewFeed { get; set; }

        public string? MetaKey { get; set; }

        public string? MetaDesc { get; set; }

        public int? Views { get; set; }
    }
}
