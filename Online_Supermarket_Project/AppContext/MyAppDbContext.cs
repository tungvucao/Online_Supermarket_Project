using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.Models;
using System.Collections.Generic;

namespace Online_Supermarket_Project.AppContext
{
    public class MyAppDbContext : DbContext
    {
        public DbSet<Customers> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SupperMarketDB;Trusted_Connection=true;");
        }
    }
}
