namespace Q10_TechnicalTest.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
