using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Models;

namespace Q10_TechnicalTest.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Subjects)
            .WithMany(s => s.Students);

        base.OnModelCreating(modelBuilder);
    }
}
