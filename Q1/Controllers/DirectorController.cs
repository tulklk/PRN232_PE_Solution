using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Data;
using Q1.DTOs;
using Q1.Models;

namespace Q1.Controllers;

[ApiController]
[Route("api/director")]
public class DirectorController : ControllerBase
{
    private readonly PE_PRN_Fall22B1Context _context;

    public DirectorController(PE_PRN_Fall22B1Context context)
    {
        _context = context;
    }

    private static string FormatDob(DateOnly dob)
    {
        // Required format: yyyy-MM-ddTHH:mm:ss
        var dt = dob.ToDateTime(TimeOnly.MinValue);
        return dt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
    }

    private static string FormatDobString(DateOnly dob)
    {
        // Required format: M/d/yyyy
        var dt = dob.ToDateTime(TimeOnly.MinValue);
        return dt.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
    }

    // GET /api/director/getdirectors/{nationality}/{gender}
    [HttpGet("getdirectors/{nationality}/{gender}")]
    public async Task<ActionResult<IEnumerable<object>>> GetDirectors(string nationality, string gender)
    {
        nationality = nationality?.Trim() ?? string.Empty;
        gender = gender?.Trim() ?? string.Empty;

        bool maleFlag = gender.Equals("male", StringComparison.OrdinalIgnoreCase);
        string nationalityUpper = nationality.ToUpperInvariant();

        var directors = await _context.Directors
            .Where(d => d.Male == maleFlag && d.Nationality.ToUpper() == nationalityUpper)
            .ToListAsync();

        var result = directors.Select(d => new
        {
            id = d.Id,
            fullName = d.FullName,
            gender = d.Male ? "Male" : "Female",
            dob = FormatDob(d.Dob),
            dobString = FormatDobString(d.Dob),
            nationality = d.Nationality,
            description = d.Description
        });

        return Ok(result);
    }

    // GET /api/director/getdirector/{id}
    [HttpGet("getdirector/{id:int}")]
    public async Task<ActionResult<object>> GetDirector(int id)
    {
        var director = await _context.Directors
            .FirstOrDefaultAsync(d => d.Id == id);

        if (director == null)
        {
            return NotFound();
        }

        var movies = await _context.Movies
            .Where(m => m.DirectorId == director.Id)
            .Include(m => m.Producer)
            .ToListAsync();

        var response = new
        {
            id = director.Id,
            fullName = director.FullName,
            gender = director.Male ? "Male" : "Female",
            dob = FormatDob(director.Dob),
            dobString = FormatDobString(director.Dob),
            nationality = director.Nationality,
            description = director.Description,
            movies = movies.Select(m => new
            {
                id = m.Id,
                title = m.Title,
                releaseDate = m.ReleaseDate.HasValue
                    ? m.ReleaseDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : null,
                releaseYear = m.ReleaseDate.HasValue ? m.ReleaseDate.Value.Year : 0,
                description = m.Description ?? string.Empty,
                language = m.Language,
                producerId = m.ProducerId,
                directorId = director.Id,
                producerName = m.Producer != null ? m.Producer.Name : string.Empty,
                directorName = director.FullName,
                genres = new List<object>(),
                stars = new List<object>()
            }).ToList()
        };

        return Ok(response);
    }

    // POST /api/director/create
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateDirectorRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        try
        {
            var dob = DateOnly.ParseExact(request.Dob, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var director = new Director
            {
                FullName = request.FullName,
                Male = request.Male,
                Dob = dob,
                Nationality = request.Nationality,
                Description = request.Description
            };

            _context.Directors.Add(director);
            int added = await _context.SaveChangesAsync();
            return Ok(added);
        }
        catch
        {
            return Conflict("There is an error while adding.");
        }
    }
}

