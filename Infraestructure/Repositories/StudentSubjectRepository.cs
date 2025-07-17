
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Infraestructure.Data;
using Q10_TechnicalTest.Infraestructure.Interfaces;

namespace Q10_TechnicalTest.Infraestructure.Repositories;
public class StudentSubjectRepository : IStudentSubjectRepository
{
    private readonly ApplicationDbContext _context;

    public StudentSubjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentSubject> Get(int studentId, int subjectId)
    {
        return await _context.StudentSubjects
            .FirstOrDefaultAsync(ss => ss.StudentId == studentId && ss.SubjectId == subjectId);
    }

    public async Task<bool> Exists(int studentId, int subjectId)
    {
        return await _context.StudentSubjects
            .AnyAsync(ss => ss.StudentId == studentId && ss.SubjectId == subjectId);
    }

    public async Task Add(StudentSubject enrollment)
    {
        await _context.StudentSubjects.AddAsync(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(StudentSubject enrollment)
    {
        _context.StudentSubjects.Remove(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Subject>> GetSubjectsByStudent(int studentId, Expression<Func<Subject, bool>> predicate = null)
    {
        var query = _context.StudentSubjects
            .Where(ss => ss.StudentId == studentId)
            .Include(ss => ss.Subject)
            .Select(ss => ss.Subject);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Subject>> GetAvailableSubjectsForStudent(int studentId)
    {
        var enrolledSubjectIds = await _context.StudentSubjects
            .Where(ss => ss.StudentId == studentId)
            .Select(ss => ss.SubjectId)
            .ToListAsync();

        return await _context.Subjects
            .Where(s => !enrolledSubjectIds.Contains(s.Id))
            .ToListAsync();
    }

    public async Task<int> CountHighCreditSubjects(int studentId)
    {
        return await _context.StudentSubjects
            .Where(ss => ss.StudentId == studentId)
            .Include(ss => ss.Subject)
            .CountAsync(ss => ss.Subject.Credits > 4);
    }
}
