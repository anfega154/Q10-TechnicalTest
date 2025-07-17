
using Q10_TechnicalTest.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Domain.Interfaces;
using Q10_TechnicalTest.Infraestructure.Data;
using Q10_TechnicalTest.Utils;

namespace Q10_TechnicalTest.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> Create(StudentDto studentDto)
        {
            var student = new Student
            {
                Name = studentDto.Name,
                Document = studentDto.Document,
                Email = studentDto.Email
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> Update(StudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(studentDto.Id);
            if (student == null)
            {
                throw new Exception($"Student with ID {studentDto.Id} not found.");
            }

            student.Name = studentDto.Name;
            student.Document = studentDto.Document;
            student.Email = studentDto.Email;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                throw new Exception($"Student with ID {id} not found.");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<PaginatedList<Student>> GetPaginatedList(int pageNumber, int pageSize, string searchString = null)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(s =>
                    s.Name.Contains(searchString) ||
                    s.Document.Contains(searchString) ||
                    s.Email.Contains(searchString));
            }

            var count = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Student>(items, count, pageNumber, pageSize);
        }
    }
}
