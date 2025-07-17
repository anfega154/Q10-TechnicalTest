using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Domain.Entities;

namespace Q10_TechnicalTest.Infraestructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Students.db",
                b => b.MigrationsAssembly("Q10_TechnicalTest.Infraestructure"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasIndex(s => s.Code).IsUnique();
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Code).IsRequired().HasMaxLength(20);
            entity.Property(s => s.Credits).IsRequired();
        });

        modelBuilder.Entity<StudentSubject>()
             .ToTable("StudentSubject")
            .HasKey(sc => new { sc.StudentId, sc.SubjectId });

        modelBuilder.Entity<StudentSubject>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentSubject>()
            .HasOne(sc => sc.Subject)
            .WithMany(c => c.StudentSubjects)
            .HasForeignKey(sc => sc.SubjectId);
    }
}
