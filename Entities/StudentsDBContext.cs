using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities;

public class StudentsDBContext : DbContext
{
    public StudentsDBContext(DbContextOptions<StudentsDBContext> options) : base(options)
    {
        
    }

    public DbSet<Student> Student { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<User> Users { get; set; }
    
}