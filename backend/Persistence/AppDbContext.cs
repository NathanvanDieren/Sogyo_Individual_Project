namespace Persistence;

using Microsoft.EntityFrameworkCore;
using Domain;


internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email)
                .IsUnique();
            
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Host=localhost;Database=ip_tastebuds;Username=IP_db_owner;Password=Wachtwoord123!";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}