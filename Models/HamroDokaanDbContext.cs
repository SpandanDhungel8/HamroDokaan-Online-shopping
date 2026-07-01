using Microsoft.EntityFrameworkCore;
namespace HamroDokaan.Models
{
    public class HamroDokaanDbContext:DbContext
    {
        public HamroDokaanDbContext(DbContextOptions<HamroDokaanDbContext> options) : base(options) { }
      
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
