
using System.ComponentModel.DataAnnotations;

namespace Q10_TechnicalTest.Application.DTO;

public class EnrollmentRequestDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int SubjectId { get; set; }
}