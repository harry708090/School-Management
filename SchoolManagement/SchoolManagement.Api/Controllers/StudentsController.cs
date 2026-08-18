using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.Data;
using SchoolManagement.Api.DTOs;
using SchoolManagement.Api.Models;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public StudentsController(SchoolDbContext context)
    {
        _context = context;
    }



[HttpGet]
public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
{
    var students = await _context.Students
        .Include(s => s.SchoolClass)
        .Include(s => s.Subjects)
        .ToListAsync();

    return Ok(students);
}


[HttpGet("{id}")]
public async Task<ActionResult<Student>> GetStudent(int id)
{
    var student = await _context.Students
        .Include(s => s.SchoolClass)
        .Include(s => s.Subjects)
        .FirstOrDefaultAsync(s => s.Id == id);

    if (student == null)
    {
        return NotFound();
    }

    return Ok(student);
}


[HttpPost]
public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto dto)
{
    var student = new Student
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        StudentNumber = dto.StudentNumber,
        SchoolClassId = dto.SchoolClassId
    };

    _context.Students.Add(student);

    await _context.SaveChangesAsync();

    var result = new StudentDto
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        StudentNumber = student.StudentNumber,
        SchoolClassId = student.SchoolClassId
    };

    return CreatedAtAction(
        nameof(GetStudent),
        new { id = student.Id },
        result);
}


[HttpPut("{id}")]
public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto dto)
{
    var student = await _context.Students.FindAsync(id);

    if (student == null)
    {
        return NotFound();
    }

    student.FirstName = dto.FirstName;
    student.LastName = dto.LastName;
    student.StudentNumber = dto.StudentNumber;
    student.SchoolClassId = dto.SchoolClassId;

    await _context.SaveChangesAsync();

    return NoContent();
}


[HttpDelete("{id}")]
public async Task<IActionResult> DeleteStudent(int id)
{
    var student = await _context.Students.FindAsync(id);

    if (student == null)
    {
        return NotFound();
    }

    _context.Students.Remove(student);

    await _context.SaveChangesAsync();

    return NoContent();
}


[HttpPost("{studentId}/subjects/{subjectId}")]
public async Task<IActionResult> AddSubject(
    int studentId,
    int subjectId)
{
    var student = await _context.Students
        .Include(s => s.Subjects)
        .FirstOrDefaultAsync(s => s.Id == studentId);

    if (student == null)
    {
        return NotFound("Student not found.");
    }

    var subject = await _context.Subjects
        .FindAsync(subjectId);

    if (subject == null)
    {
        return NotFound("Subject not found.");
    }

    if (student.Subjects.Any(s => s.Id == subjectId))
    {
        return BadRequest("Student already has this subject.");
    }

    student.Subjects.Add(subject);

    await _context.SaveChangesAsync();

    return Ok();
}


[HttpGet("{studentId}/subjects")]
public async Task<ActionResult<IEnumerable<Subject>>> GetStudentSubjects(
    int studentId)
{
    var student = await _context.Students
        .Include(s => s.Subjects)
        .FirstOrDefaultAsync(s => s.Id == studentId);

    if (student == null)
    {
        return NotFound();
    }

    return Ok(student.Subjects);
}

[HttpDelete("{studentId}/subjects/{subjectId}")]
public async Task<IActionResult> RemoveSubject(
    int studentId,
    int subjectId)
{
    var student = await _context.Students
        .Include(s => s.Subjects)
        .FirstOrDefaultAsync(s => s.Id == studentId);

    if (student == null)
    {
        return NotFound();
    }

    var subject = student.Subjects
        .FirstOrDefault(s => s.Id == subjectId);

    if (subject == null)
    {
        return NotFound("Subject is not assigned to this student.");
    }

    student.Subjects.Remove(subject);

    await _context.SaveChangesAsync();

    return NoContent();
}
}