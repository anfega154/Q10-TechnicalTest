using Q10_TechnicalTest.Domain.Entities;

namespace Q10_TechnicalTest.Domain.Interfaces;

public interface ISubjectService
{
    Task<IEnumerable<Subject>> GetAll();
    Task<Subject> GetById(int id);
    Task<Subject> Create(Subject course);
    Task<Subject> Update(Subject course);
    Task Delete(int id);
    Task<bool> Exists(int id);
    IQueryable<Subject> GetAllQueryable();
    Task<IEnumerable<Subject>> GetAvailableSubjects(int studentId);
}