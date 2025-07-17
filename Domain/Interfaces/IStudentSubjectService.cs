
using Q10_TechnicalTest.Application.DTO;

namespace Q10_TechnicalTest.Domain.Interfaces;
public interface IStudentSubjectService
{
    Task EnrollStudent(int studentId, int subjectId);
    Task UnenrollStudent(int studentId, int subjectId);
    Task<IEnumerable<SubjectDto>> GetEnrolledSubjects(int studentId);
    Task<IEnumerable<SubjectDto>> GetAvailableSubjects(int studentId);
    Task<EnrollmentSummaryDto> GetEnrollmentSummary(int studentId);
}