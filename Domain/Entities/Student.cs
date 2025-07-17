namespace Q10_TechnicalTest.Domain.Entities;

using System.ComponentModel.DataAnnotations;
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }


    public string Document { get; set; }

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; }
    public ICollection<StudentSubject> StudentSubjects { get; set; } = new HashSet<StudentSubject>();
}
