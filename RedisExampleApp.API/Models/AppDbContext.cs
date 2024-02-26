using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed Data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Kalem", Price = 100 },
                new Product { Id = 2, Name = "Kalem2", Price = 200 },
                new Product { Id = 3, Name = "Kalem3", Price = 300 }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
