using Q10_TechnicalTest.Domain.Entities;

namespace Q10_TechnicalTest.Infraestructure.Interfaces;
public interface IStudentRepository
{
    Task<Student> GetById(int id);
    IQueryable<Student> GetAll();
    Task Add(Student student);
    Task Update(Student student);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task<Student?> GetByEmail(string email);
    Task<Student?> DocumentExist(string document);
    Task<Student> GetByIdWithSubjects(int id);
    Task DeleteStudentSubjects(ICollection<StudentSubject> studentSubjects);
}