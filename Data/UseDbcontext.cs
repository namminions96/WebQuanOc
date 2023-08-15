using CheckApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Report.Data
{
    public class UseDbcontext:DbContext
    {
  public DbSet<Item> Items { get; set; }
        public DbSet<cart> Carts { get; set; }
        public DbSet<CartSave> cartSaves { get; set; }
        public UseDbcontext(DbContextOptions<UseDbcontext> options)
                   : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
}
