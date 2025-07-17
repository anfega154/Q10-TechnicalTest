using Q10_TechnicalTest.Application.DTO;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Domain.Interfaces;
using Q10_TechnicalTest.Infraestructure.Interfaces;

namespace Q10_TechnicalTest.Application.Services;

public class StudentSubjectService : IStudentSubjectService
{
    private readonly IStudentSubjectRepository _studentSubjectRepo;
    private readonly IStudentRepository _studentRepo;
    private readonly ISubjectRepository _subjectRepo;

    public StudentSubjectService(
        IStudentSubjectRepository studentSubjectRepo,
        IStudentRepository studentRepo,
        ISubjectRepository subjectRepo)
    {
        _studentSubjectRepo = studentSubjectRepo;
        _studentRepo = studentRepo;
        _subjectRepo = subjectRepo;
    }

    public async Task EnrollStudent(int studentId, int subjectId)
    {
        var student = await _studentRepo.GetById(studentId)
            ?? throw new DomainException("Estudiante no encontrado");
        var subject = await _subjectRepo.GetById(subjectId)
            ?? throw new DomainException("Materia no encontrada");

        if (await _studentSubjectRepo.Exists(studentId, subjectId))
            throw new DomainException("El estudiante ya está inscrito en esta materia");

        await ValidateCreditLimit(studentId, subject);

        var enrollment = new StudentSubject
        {
            StudentId = studentId,
            SubjectId = subjectId
        };

        await _studentSubjectRepo.Add(enrollment);
    }

    private async Task ValidateCreditLimit(int studentId, Subject newSubject)
    {
        var highCreditSubjects = await _studentSubjectRepo
            .GetSubjectsByStudent(studentId, s => s.Credits > 4);

        if (newSubject.Credits > 4 && highCreditSubjects.Count() >= 3)
        {
            throw new DomainException("No puede inscribir más de 3 materias con más de 4 créditos");
        }
    }

    public async Task UnenrollStudent(int studentId, int subjectId)
    {
        var enrollment = await _studentSubjectRepo.Get(studentId, subjectId)
            ?? throw new DomainException("Inscripción no encontrada");

        await _studentSubjectRepo.Delete(enrollment);
    }

    public async Task<IEnumerable<SubjectDto>> GetEnrolledSubjects(int studentId)
    {
        var subjects = await _studentSubjectRepo.GetSubjectsByStudent(studentId);
        return subjects.Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            Code = s.Code,
            Credits = s.Credits
        }).ToList();
    }

    public async Task<IEnumerable<SubjectDto>> GetAvailableSubjects(int studentId)
    {
        var subjects = await _studentSubjectRepo.GetAvailableSubjectsForStudent(studentId);
        return subjects.Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            Code = s.Code,
            Credits = s.Credits
        }).ToList();
    }

    public async Task<EnrollmentSummaryDto> GetEnrollmentSummary(int studentId)
    {
        var highCreditCount = await _studentSubjectRepo.CountHighCreditSubjects(studentId);
        var totalSubjects = (await _studentSubjectRepo.GetSubjectsByStudent(studentId)).Count();

        return new EnrollmentSummaryDto
        {
            TotalSubjects = totalSubjects,
            HighCreditSubjects = highCreditCount,
            CanEnrollMoreHighCredit = highCreditCount < 3
        };
    }

}