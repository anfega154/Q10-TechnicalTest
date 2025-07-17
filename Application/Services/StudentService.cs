using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Infraestructure.Interfaces;
using Q10_TechnicalTest.Utils;
using Q10_TechnicalTest.Domain.Interfaces;

namespace Q10_TechnicalTest.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Student>> GetAll()
        => await _repository.GetAll().ToListAsync();

    public async Task<Student> GetById(int id)
        => await _repository.GetById(id)
            ?? throw new DomainException("Estudiante no encontrado");

    public async Task<Student> Create(StudentDto studentDto)
    {
        ValidateStudent(studentDto);
        await ValidateUniqueConstraints(studentDto);

        var student = new Student
        {
            Name = studentDto.Name,
            Document = studentDto.Document,
            Email = studentDto.Email
        };

        await _repository.Add(student);
        return student;
    }

    public async Task<Student> Update(StudentDto studentDto)
    {
        var existingStudent = await _repository.GetById(studentDto.Id)
            ?? throw new DomainException("Estudiante no encontrado");

        ValidateStudent(studentDto);
        await ValidateUniqueConstraints(studentDto);

        existingStudent.Name = studentDto.Name;
        existingStudent.Document = studentDto.Document;
        existingStudent.Email = studentDto.Email;

        await _repository.Update(existingStudent);
        return existingStudent;
    }

    public async Task Delete(int id)
    {
        var student = await _repository.GetByIdWithSubjects(id)
            ?? throw new DomainException("Estudiante no encontrado");

        if (student.StudentSubjects?.Any() == true)
        {
            await _repository.DeleteStudentSubjects(student.StudentSubjects);
        }

        await _repository.Delete(id);
    }

    public async Task<bool> Exists(int id)
        => await _repository.Exists(id);

    public async Task<PaginatedList<Student>> GetPaginatedList(int pageNumber, int pageSize, string searchString = null)
    {
        var query = _repository.GetAll();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(s =>
                s.Name.Contains(searchString) ||
                s.Email.Contains(searchString) ||
                s.Document.Contains(searchString));
        }

        return await PaginatedList<Student>.CreateAsync(query, pageNumber, pageSize, searchString);
    }

    private void ValidateStudent(StudentDto student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new DomainException("El nombre es requerido");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new DomainException("El email es requerido");

        if (!new EmailAddressAttribute().IsValid(student.Email))
            throw new DomainException("Email no válido");

        if (string.IsNullOrWhiteSpace(student.Document))
            throw new DomainException("El documento es requerido");
    }

    private async Task ValidateUniqueConstraints(StudentDto student)
    {
       var existingStudentDoc = await _repository.DocumentExist(student.Document);
        if (existingStudentDoc != null && existingStudentDoc.Id != student.Id)
            throw new DomainException($"Ya existe un usuario con el documento: {student.Document}");

        var existingStudentEmail = await _repository.GetByEmail(student.Email);
        if (existingStudentEmail != null && existingStudentEmail.Id != student.Id)
            throw new DomainException("El email ya está registrado");
    }

    Task<PaginatedList<Student>> IStudentService.GetPaginatedList(int pageNumber, int pageSize, string searchString)
    {
        throw new NotImplementedException();
    }
}