using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public EventController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        return await _context.Events
            .Include(e => e.Points)
                .ThenInclude(p => p.Room)
                    .ThenInclude(r => r.Floor)
                        .ThenInclude(f => f.Building)
            .ToListAsync();
    }
}