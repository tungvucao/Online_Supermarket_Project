using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Supermarket_Project.Models
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string state { get; set; }

        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardExpiry { get; set; }
    }
}
