
using Q10_TechnicalTest.Application.DTO;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Utils;

namespace Q10_TechnicalTest.Domain.Interfaces;
public interface IStudentService
{
    Task<IEnumerable<Student>> GetAll();
    Task<Student> GetById(int id);
    Task<Student> Create(StudentDto studentDto);
    Task<Student> Update(StudentDto studentDto);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task<PaginatedList<Student>> GetPaginatedList(int pageNumber, int pageSize, string searchString = null);
}