using Domain.Classes;

namespace Persistence;

using Microsoft.EntityFrameworkCore;
using Domain;


internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Review> Reviews { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email)
                .IsUnique();
            
        });
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
            
            entity.HasOne(g => g.Creator)
                .WithMany()
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(g => g.Members)
                .WithMany()
                .UsingEntity(j => j.ToTable("GroupMembers"));

            entity.Navigation(g => g.Members)
                .HasField("_members")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });
        
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Title).IsRequired().HasMaxLength(150);
            
            entity.Property(r => r.ItemType)
                .HasConversion<string>();
            entity.HasMany(r => r.Groups)
                .WithMany() 
                .UsingEntity(j => j.ToTable("GroupReviews"));

            entity.Navigation(r => r.Groups)
                .HasField("_groups")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
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