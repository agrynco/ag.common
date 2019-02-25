using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class SpecificDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SpecificDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b => { b.HasIndex(e => new {e.Email}).IsUnique(); });
        }
    }
}