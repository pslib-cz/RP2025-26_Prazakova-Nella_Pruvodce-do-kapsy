using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;


[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReferenceDataController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("teachers")]
    public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
    {
        return await _context.Teachers.ToListAsync();
    }

    [HttpGet("subjects")]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
    {
        return await _context.Subjects.ToListAsync();
    }

    [HttpGet("points")]
    public async Task<ActionResult<IEnumerable<Point>>> GetPoints()
    {
        return await _context.Points
            .Include(p => p.Teacher)
            .Include(p => p.Subjects)
            .ToListAsync();
    }
}