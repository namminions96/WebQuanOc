using CheckApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Report.Data
{
    public class UseDbcontext:DbContext
    {
  public DbSet<Item> Items { get; set; }
        public DbSet<cart> Carts { get; set; }
        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<ConfigOrder> ConfigOrders { get; set; }
        public DbSet<login> Logins { get; set; }
        public DbSet<CreateUser> CreateUsers { get; set; }
        public UseDbcontext(DbContextOptions<UseDbcontext> options)
                   : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
}
