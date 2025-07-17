using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Infraestructure.Data;
using Q10_TechnicalTest.Infraestructure.Interfaces;

namespace Q10_TechnicalTest.Infraestructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> GetById(int id)
        => await _context.Students.FindAsync(id);

    public IQueryable<Student> GetAll()
        => _context.Students.AsQueryable();

    public async Task Add(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var student = await GetById(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exists(int id)
        => await _context.Students.AnyAsync(e => e.Id == id);

    public async Task<Student?> GetByEmail(string email)
    {
        return await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<Student?> DocumentExist(string document)
    {
        return await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Document == document);
    }

    public async Task<Student> GetByIdWithSubjects(int id)
    {
        return await _context.Students
            .Include(s => s.StudentSubjects)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task DeleteStudentSubjects(ICollection<StudentSubject> studentSubjects)
    {
        _context.StudentSubjects.RemoveRange(studentSubjects);
        await _context.SaveChangesAsync();
    }
}