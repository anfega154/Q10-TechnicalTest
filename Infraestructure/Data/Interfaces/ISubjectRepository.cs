using Q10_TechnicalTest.Domain.Entities;

namespace Q10_TechnicalTest.Infraestructure.Interfaces;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAll();
    Task<Subject> GetById(int id);
    Task<Subject> GetByCode(string code);
    Task<Subject> Add(Subject subject);
    Task<Subject> Update(Subject subject);
    Task Delete(int id);
    Task<bool> Exists(int id);
    IQueryable<Subject> GetAllQueryable();
    Task<Subject> GetByIdWithEnrollments(int id);
}
