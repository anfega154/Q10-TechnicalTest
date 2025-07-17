    using Q10_TechnicalTest.Application.DTO;
    using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Domain.Interfaces;
using Q10_TechnicalTest.Infraestructure.Interfaces;

namespace Q10_TechnicalTest.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly IStudentRepository _studentRepository;

    public SubjectService(ISubjectRepository subjectRepository, IStudentRepository studentRepository)
    {
        _subjectRepository = subjectRepository;
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<Subject>> GetAll()
    {
        return await _subjectRepository.GetAll();
    }

    public IQueryable<Subject> GetAllQueryable()
    {
        return _subjectRepository.GetAllQueryable();
    }

    public async Task<Subject> GetById(int id)
    {
        return await _subjectRepository.GetById(id);
    }

    public async Task<Subject> Create(Subject subject)
    {
        var existingSubject = await _subjectRepository.GetByCode(subject.Code);
        if (existingSubject != null)
            throw new DomainException("El código de la materia ya existe.");

        if (subject.Credits <= 0)
            throw new DomainException("Los créditos deben ser mayores a cero.");

        return await _subjectRepository.Add(subject);
    }

    public async Task<Subject> Update(Subject subject)
    {
        if (!await _subjectRepository.Exists(subject.Id))
            throw new DomainException("Materia no encontrada.");

        if (subject.Credits <= 0)
            throw new DomainException("Los créditos deben ser mayores a cero.");

        return await _subjectRepository.Update(subject);
    }

    public async Task Delete(int id)
    {
        var subject = await _subjectRepository.GetByIdWithEnrollments(id)
            ?? throw new DomainException("Materia no encontrada");

        if (subject.StudentSubjects?.Any() == true)
        {
            throw new DomainException(
                $"No se puede eliminar la materia porque tiene {subject.StudentSubjects.Count} estudiantes matriculados. " +
                "Desasocie todos los estudiantes primero.");
        }

        await _subjectRepository.Delete(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await _subjectRepository.Exists(id);
    }

    public async Task<IEnumerable<Subject>> GetAvailableSubjects(int studentId)
    {
        var student = await _studentRepository.GetById(studentId);
        if (student == null)
            throw new System.Exception("Estudiante no encontrado.");

        var enrolledCourseIds = student.StudentSubjects.Select(sc => sc.SubjectId).ToList();

        var allCourses = await _subjectRepository.GetAll();
        return allCourses.Where(c => !enrolledCourseIds.Contains(c.Id)).ToList();
    }
}