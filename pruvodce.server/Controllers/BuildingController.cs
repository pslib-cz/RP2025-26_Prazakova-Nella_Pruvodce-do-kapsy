using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruvodce.server.Data;
using pruvodce.server.Models;

[ApiController]
[Route("api/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BuildingsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetBuildings()
    {
        var buildings = await _context.Buildings
            .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                    .ThenInclude(r => r.Points)
                        .ThenInclude(p => p.Teacher)
            .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                    .ThenInclude(r => r.Subjects)
            .ToListAsync();

        return Ok(buildings.Select(MapBuildingToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<dynamic>> GetBuilding(int id)
    {
        var building = await _context.Buildings
            .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                    .ThenInclude(r => r.Points)
                        .ThenInclude(p => p.Teacher)
            .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                    .ThenInclude(r => r.Subjects)
            .FirstOrDefaultAsync(b => b.BuildingId == id);

        if (building == null)
            return NotFound(new { message = "Budova nenalezena" });

        return Ok(MapBuildingToDto(building));
    }

    private dynamic MapBuildingToDto(Building building)
    {
        return new
        {
            buildingId = building.BuildingId,
            name = building.Name,
            address = building.Address,
            floors = building.Floors.Select(f => new
            {
                floorId = f.FloorId,
                name = f.Name,
                mapImageUrl = f.MapImageUrl,
                rooms = f.Rooms.Select(r => new
                {
                    roomId = r.RoomId,
                    svgData = r.SvgData,
                    label = r.Label,
                    icon = r.Icon,
                    coordinateX = r.CoordinateX,
                    coordinateY = r.CoordinateY,
                    type = r.Type,
                    note = r.Note,
                    floorId = r.FloorId,
                    points = r.Points.Select(p => new
                    {
                        pointId = p.PointId,
                        label = p.Label,
                        description = p.Description,
                        labelX = p.LabelX,
                        labelY = p.LabelY,
                        note = p.Note,
                        icon = p.Icon,
                        teacherId = p.TeacherId,
                        teacher = p.Teacher != null ? new
                        {
                            teacherId = p.Teacher.TeacherId,
                            degree = p.Teacher.Degree,
                            firstN = p.Teacher.FirstN,
                            lastN = p.Teacher.LastN,
                            note = p.Teacher.Note
                        } : (object)null,
                        roomId = p.RoomId,
                        eventId = p.EventId,
                        subjects = p.Subjects.Select(s => new
                        {
                            subjectId = s.SubjectId,
                            name = s.Name,
                            acronym = s.Acronym,
                            note = s.Note
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList()
        };
    }
}