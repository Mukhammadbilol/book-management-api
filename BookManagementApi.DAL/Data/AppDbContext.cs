using BookManagementApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Book>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Title)
            .IsUnique();

        modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            Username = "admin",
            Password = "admin123",
            Role = "Admin"
        },
        new User
        {
            Id = 2,
            Username = "user",
            Password = "user123",
            Role = "User"
        }
    );
    }
}
