
using System.ComponentModel.DataAnnotations;
using Q10_TechnicalTest.Domain.Entities;

namespace Q10_TechnicalTest.Application.DTO;


public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Code { get; set; }
    public int Credits { get; set; }
    public ICollection<StudentSubject> StudentSubjects { get; set; }
}