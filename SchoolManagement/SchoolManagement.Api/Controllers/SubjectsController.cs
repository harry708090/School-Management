using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Api.Data;
using SchoolManagement.Api.Models;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public SubjectsController(SchoolDbContext context)
    {
        _context = context;
    }

    // GET: api/subjects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
    {
        var subjects = await _context.Subjects
            .Include(s => s.Students)
            .Include(s => s.SchoolClass)
            .ToListAsync();

        return Ok(subjects);
    }

    // GET: api/subjects/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Subject>> GetSubject(int id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Students)
            .Include(s => s.SchoolClass)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subject == null)
        {
            return NotFound();
        }

        return Ok(subject);
    }

    // POST: api/subjects
    [HttpPost]
    public async Task<ActionResult<Subject>> CreateSubject(Subject subject)
    {
        _context.Subjects.Add(subject);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSubject),
            new { id = subject.Id },
            subject);
    }

    // PUT: api/subjects/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, Subject subject)
    {
        if (id != subject.Id)
        {
            return BadRequest();
        }

        _context.Entry(subject).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Subjects.AnyAsync(s => s.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/subjects/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);

        if (subject == null)
        {
            return NotFound();
        }

        _context.Subjects.Remove(subject);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
