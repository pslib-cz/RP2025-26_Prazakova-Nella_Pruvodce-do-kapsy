using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;
using pruvodce.server.Services;
 
namespace pruvodce.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MapDataService _mapDataService;
 
        public ReferenceDataController(ApplicationDbContext context, MapDataService mapDataService)
        {
            _context = context;
            _mapDataService = mapDataService;
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
                    t.Notes
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
                    s.ActiveNote
                })
                .ToListAsync();
 
            return Ok(subjects);
        }
 
        [HttpGet("events")]
        public async Task<ActionResult<IEnumerable<object>>> GetEvents()
        {
            var visibleEvents = await GetCurrentlyVisibleEventsAsync();
 
            var result = visibleEvents
                .OrderBy(e => e.BuildingId)
                .Select(e => new
                {
                    e.Event.EventId,
                    e.Event.Name,
                    e.Event.StartDate,
                    e.Event.EndDate,
                    e.Event.IsActive,
                    e.Event.Description,
                    e.Event.CreatedAt,
                    e.BuildingId,
                    Buildings = e.Event.EventBuildings
                        .Select(eb => eb.BuildingId)
                        .ToList()
                })
                .ToList();
 
            return Ok(result);
        }
        [HttpGet("debug")]
public async Task<IActionResult> Debug()
{
    var now = DateTime.Now;
    var events = await _context.Events
        .Include(e => e.EventBuildings)
        .ToListAsync();

    return Ok(new {
        Now = now,
        EventCount = events.Count,
        Events = events.Select(e => new {
            e.EventId,
            e.Name,
            e.IsActive,
            e.StartDate,
            e.EndDate,
            EventBuildings = e.EventBuildings.Select(eb => eb.BuildingId).ToList()
        })
    });
}
 
        [HttpGet("points")]
        public async Task<ActionResult<IEnumerable<object>>> GetPoints()
        {
            var visibleEvents = await GetCurrentlyVisibleEventsAsync();
 
            var visiblePairs = visibleEvents
                .Select(e => new { e.EventId, e.BuildingId })
                .ToList();
 
            var visibleEventIds = visiblePairs
                .Select(x => x.EventId)
                .Distinct()
                .ToList();
 
            var roomBuildingMap = await GetRoomBuildingMapAsync();
 
            var rawPoints = await _context.EventPoints
                .Where(ep => visibleEventIds.Contains(ep.EventId))
                .Include(ep => ep.Point)
                    .ThenInclude(p => p!.PointTeachers)
                        .ThenInclude(pt => pt.Teacher)
                .Include(ep => ep.Point)
                    .ThenInclude(p => p!.PointSubjects)
                        .ThenInclude(ps => ps.Subject)
                .Include(ep => ep.Point)
                    .ThenInclude(p => p!.Specialization)
                .Include(ep => ep.Event)
                    .ThenInclude(e => e!.EventBuildings)
                .AsNoTracking()
                .ToListAsync();
 
            var filteredPoints = rawPoints
                .Where(ep =>
                    ep.Point != null &&
                    ep.Point.RoomId != null &&
                    roomBuildingMap.TryGetValue(ep.Point.RoomId, out var pointBuildingId) &&
                    visiblePairs.Any(v =>
                        v.EventId == ep.EventId &&
                        v.BuildingId == pointBuildingId))
                .Select(ep => ep.Point!)
                .DistinctBy(p => p.PointId)
                .ToList();
 
            var result = filteredPoints
                .Select(p => new
                {
                    p.PointId,
                    p.Label,
                    p.Description,
                    p.Icon,
                    p.RoomId,
                    p.AreStudents,
 
                    p.SpecializationId,
                    Specialization = p.Specialization == null ? null : new
                    {
                        p.Specialization.SpecializationId,
                        p.Specialization.Name,
                        p.Specialization.Description,
                        p.Specialization.Type
                    },
 
                    Teachers = p.PointTeachers.Select(pt => new
                    {
                        pt.TeacherId,
                        pt.Teacher.Degree,
                        pt.Teacher.FirstN,
                        pt.Teacher.LastN,
                        pt.Teacher.Notes
                    }).ToList(),
 
                    Subjects = p.PointSubjects.Select(ps => new
                    {
                        ps.Subject.SubjectId,
                        ps.Subject.Name,
                        ps.Subject.Acronym,
                        ps.Subject.ActiveNote
                    }).ToList()
                })
                .ToList();
 
            return Ok(result);
        }
 
        private async Task<List<VisibleEventForBuilding>> GetCurrentlyVisibleEventsAsync()
        {
            var now = DateTime.Now;
 
            var events = await _context.Events
                .Include(e => e.EventBuildings)
                .AsNoTracking()
                .ToListAsync();
 
            var eventBuildingRows = events
                .SelectMany(e => e.EventBuildings.Select(eb => new
                {
                    Event = e,
                    eb.BuildingId
                }))
                .ToList();
 
            var visibleEvents = eventBuildingRows
                .GroupBy(x => x.BuildingId)
                .Select(group =>
                {
                    var currentlyRunning = group
                        .Where(x => 
                            (!x.Event.StartDate.HasValue || x.Event.StartDate <= now) &&
                            (!x.Event.EndDate.HasValue || x.Event.EndDate >= now) &&
                            x.Event.IsActive)
                        .OrderByDescending(x => x.Event.CreatedAt)
                        .FirstOrDefault();
 
                    if (currentlyRunning != null)
                    {
                        return new VisibleEventForBuilding
                        {
                            EventId = currentlyRunning.Event.EventId,
                            Event = currentlyRunning.Event,
                            BuildingId = currentlyRunning.BuildingId
                        };
                    }
 
                    var manuallyActive = group
                        .Where(x => x.Event.IsActive &&
                            (!x.Event.StartDate.HasValue || x.Event.StartDate <= now) &&
                            (!x.Event.EndDate.HasValue || x.Event.EndDate >= now))
                        .OrderByDescending(x => x.Event.CreatedAt)
                        .FirstOrDefault();
 
                    if (manuallyActive == null)
                    {
                        return null;
                    }
 
                    return new VisibleEventForBuilding
                    {
                        EventId = manuallyActive.Event.EventId,
                        Event = manuallyActive.Event,
                        BuildingId = manuallyActive.BuildingId
                    };
                })
                .Where(x => x != null)
                .Cast<VisibleEventForBuilding>()
                .ToList();
 
            return visibleEvents;
        }
 
        private async Task<Dictionary<string, int>> GetRoomBuildingMapAsync()
        {
            var mapData = await _mapDataService.GetMapDataAsync();
 
            return mapData.Buildings
                .SelectMany(building =>
                    building.Floors.SelectMany(floor =>
                        floor.Rooms.Select(room => new
                        {
                            room.RoomId,
                            building.BuildingId
                        })))
                .Where(x => !string.IsNullOrWhiteSpace(x.RoomId))
                .GroupBy(x => x.RoomId)
                .ToDictionary(g => g.Key, g => g.First().BuildingId);
        }
 
        private class VisibleEventForBuilding
        {
            public int EventId { get; set; }
            public int BuildingId { get; set; }
            public Event Event { get; set; } = default!;
        }
    }
}