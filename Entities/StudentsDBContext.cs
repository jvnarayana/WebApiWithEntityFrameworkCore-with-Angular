using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities;

public class StudentsDBContext : DbContext
{
    public StudentsDBContext(DbContextOptions<StudentsDBContext> options) : base(options)
    {
        
    }

    public DbSet<Student> Student { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<User> User { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configure Student entity
    modelBuilder.Entity<Student>(entity =>
    {
        entity.HasKey(e => e.Id); // Primary key

        entity.Property(e => e.FirstName)
            .HasMaxLength(100) // Optional length
            .IsRequired(false); // Allow null

        entity.Property(e => e.LastName)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.City)
            .HasMaxLength(100)
            .IsRequired(false);

        // Configure relationship with Address (Foreign Key AddressId)
        entity.HasOne(s => s.Address)
            .WithMany()
            .HasForeignKey(s => s.AddressId)
            .OnDelete(DeleteBehavior.Restrict); // Optional: Define delete behavior
    });

    // Configure Address entity
    modelBuilder.Entity<Address>(entity =>
    {
        entity.HasKey(e => e.AddressId); // Primary key

        entity.Property(e => e.StreetNumber)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.City)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.State)
            .HasMaxLength(50)
            .IsRequired(false);

        entity.Property(e => e.Country)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.Zipcode)
            .IsRequired(false); // Allow null
    });

    // Configure Teacher entity
    modelBuilder.Entity<Teacher>(entity =>
    {
        entity.HasKey(e => e.Id); // Primary key

        entity.Property(e => e.FirstName)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.LastName)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.City)
            .HasMaxLength(100)
            .IsRequired(false);

        // Configure relationship with Address (Foreign Key AddressId)
        entity.HasOne(t => t.Address)
            .WithMany()
            .HasForeignKey(s=>s.AddressId)
            .OnDelete(DeleteBehavior.Restrict); // Optional: Define delete behavior
    });

    // Configure User entity
    modelBuilder.Entity<User>(entity =>
    {
        entity.HasKey(e => e.Id); // Primary key

        entity.Property(e => e.UserName)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(e => e.PasswordHash)
            .HasMaxLength(200) // Allow a longer length for hashed passwords
            .IsRequired(false);
    });

    base.OnModelCreating(modelBuilder);
}

    
}