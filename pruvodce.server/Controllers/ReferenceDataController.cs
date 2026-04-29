using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;

namespace pruvodce.server.Controllers
{
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
        public async Task<ActionResult<IEnumerable<object>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .OrderBy(t => t.LastN)
                .ThenBy(t => t.FirstN)
                .Select(t => new
                {
                    t.TeacherId,
                    t.Degree,
                    t.FirstN,
                    t.LastN,
                    t.Note
                })
                .ToListAsync();

            return Ok(teachers);
        }

        [HttpGet("subjects")]
        public async Task<ActionResult<IEnumerable<object>>> GetSubjects()
        {
            var subjects = await _context.Subjects
                .OrderBy(s => s.Name)
                .Select(s => new
                {
                    s.SubjectId,
                    s.Name,
                    s.Acronym,
                    s.Note
                })
                .ToListAsync();

            return Ok(subjects);
        }

        [HttpGet("events")]
        public async Task<ActionResult<IEnumerable<object>>> GetEvents()
        {
            var events = await _context.Events
                .OrderBy(e => e.StartDate)
                .Select(e => new
                {
                    e.EventId,
                    e.Name,
                    e.StartDate,
                    e.EndDate,
                    e.IsActive,
                    e.Description,
                    e.BuildingId
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("points")]
        public async Task<ActionResult<IEnumerable<object>>> GetPoints()
        {
            var points = await _context.Points
                .Include(p => p.Teachers)
                .Include(p => p.Subjects)
                .Include(p => p.Event)
                .Include(p => p.Specialization)
                .AsNoTracking()
                .Select(p => new
                {
                    p.PointId,
                    p.Label,
                    p.Description,
                    p.Note,
                    p.Icon,
                    p.RoomId,

                    p.EventId,
                    Event = p.Event == null ? null : new
                    {
                        p.Event.EventId,
                        p.Event.Name,
                        p.Event.StartDate,
                        p.Event.EndDate,
                        p.Event.IsActive,
                        p.Event.Description,
                        p.Event.BuildingId
                    },

                    p.SpecializationId,
                    Specialization = p.Specialization == null ? null : new
                    {
                        p.Specialization.SpecializationId,
                        p.Specialization.Name,
                        p.Specialization.Description,
                        p.Specialization.Type,
                        p.Specialization.Icon
                    },

                    Teachers = p.Teachers.Select(t => new
                    {
                        t.TeacherId,
                        t.Degree,
                        t.FirstN,
                        t.LastN,
                        t.Note
                    }).ToList(),

                    Subjects = p.Subjects.Select(s => new
                    {
                        s.SubjectId,
                        s.Name,
                        s.Acronym,
                        s.Note
                    }).ToList()
                })
                .ToListAsync();

            return Ok(points);
        }
    }
}