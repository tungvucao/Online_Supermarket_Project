using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.Models;
using System.Collections.Generic;

namespace Online_Supermarket_Project.AppContext
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<AttributesPrice> AttributesPrice { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<New> New { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Shipper> Shipper { get; set; }
        public DbSet<TransactStatus> TransactStatus { get; set; }
        public DbSet<Customer> Customers  { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SupperMarketDB;Trusted_Connection=true;TrustServerCertificate=true;");
        //}
    }
}
