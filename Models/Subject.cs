namespace Q10_TechnicalTest.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
}