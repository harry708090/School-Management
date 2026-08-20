namespace SchoolManagement.Api.Models;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int SchoolClassId { get; set; }

    public SchoolClass? SchoolClass { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
}