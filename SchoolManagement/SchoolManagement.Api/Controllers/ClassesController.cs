using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.Data;
using SchoolManagement.Api.Models;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public ClassesController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolClass>>> GetClasses()
    {
        var classes = await _context.Classes
            .Include(c => c.Students)
            .Include(c => c.Subjects)
            .ToListAsync();

        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchoolClass>> GetClass(int id)
    {
        var schoolClass = await _context.Classes
            .Include(c => c.Students)
            .Include(c => c.Subjects)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (schoolClass == null)
        {
            return NotFound();
        }

        return Ok(schoolClass);
    }

    [HttpPost]
    public async Task<ActionResult<SchoolClass>> CreateClass(SchoolClass schoolClass)
    {
        _context.Classes.Add(schoolClass);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetClass),
            new { id = schoolClass.Id },
            schoolClass);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(
        int id,
        SchoolClass schoolClass)
    {
        if (id != schoolClass.Id)
        {
            return BadRequest();
        }

        _context.Entry(schoolClass).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Classes.AnyAsync(c => c.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var schoolClass = await _context.Classes.FindAsync(id);

        if (schoolClass == null)
        {
            return NotFound();
        }

        _context.Classes.Remove(schoolClass);

        await _context.SaveChangesAsync();

        return NoContent();
    }



[HttpGet("{id}/students")]
public async Task<ActionResult<IEnumerable<Student>>> GetClassStudents(int id)
{
    var schoolClass = await _context.Classes
        .Include(c => c.Students)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (schoolClass == null)
    {
        return NotFound();
    }

    return Ok(schoolClass.Students);
}

[HttpGet("{id}/subjects")]
public async Task<ActionResult<IEnumerable<Subject>>> GetClassSubjects(int id)
{
    var schoolClass = await _context.Classes
        .Include(c => c.Subjects)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (schoolClass == null)
    {
        return NotFound();
    }

    return Ok(schoolClass.Subjects);
}
 
[HttpGet("{id}/students/{studentId}")]
public async Task<ActionResult<Student>> GetClassStudent(
    int id,
    int studentId)
{
    var student = await _context.Students
        .Include(s => s.Subjects)
        .FirstOrDefaultAsync(s =>
            s.Id == studentId &&
            s.SchoolClassId == id);

    if (student == null)
    {
        return NotFound();
    }

    return Ok(student);
}
}
