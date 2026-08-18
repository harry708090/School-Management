namespace SchoolManagement.Api.Models;

public class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string StudentNumber { get; set; } = string.Empty;

    public int SchoolClassId { get; set; }

    public SchoolClass? SchoolClass { get; set; } = null!;

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}