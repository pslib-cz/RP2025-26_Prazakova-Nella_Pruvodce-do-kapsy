using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public EventController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Events.Include(e => e.Points).ToListAsync();
        return Ok(events);
    }
}