using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rasket.Api.Data;
using Rasket.Api.DTOs;
using Rasket.Api.Models;

namespace Rasket.Api.Controllers;

[ApiController]
[Route("api/calendar")]
public class CalendarController : ControllerBase
{
    private readonly AppDbContext _context;

    public CalendarController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetEvents()
    {
        var events = await _context.CalendarEvents.AsNoTracking().ToListAsync();
        return Ok(events.Select(e => new CalendarEventDto
        {
            Id = e.Id,
            Title = e.Title,
            Start = e.Start,
            End = e.End,
            ClassName = e.ClassName,
            AllDay = e.AllDay
        }));
    }

    [HttpPost]
    public async Task<ActionResult<CalendarEventDto>> CreateEvent([FromBody] CalendarEventDto dto)
    {
        var newEvent = new CalendarEvent
        {
            Id = string.IsNullOrWhiteSpace(dto.Id) ? System.Guid.NewGuid().ToString() : dto.Id,
            Title = dto.Title,
            Start = dto.Start,
            End = dto.End,
            ClassName = dto.ClassName,
            AllDay = dto.AllDay
        };

        _context.CalendarEvents.Add(newEvent);
        await _context.SaveChangesAsync();

        dto.Id = newEvent.Id;
        return CreatedAtAction(nameof(GetEvents), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(string id, [FromBody] CalendarEventDto dto)
    {
        var existing = await _context.CalendarEvents.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Event not found" });

        existing.Title = dto.Title;
        existing.Start = dto.Start;
        existing.End = dto.End;
        existing.ClassName = dto.ClassName;
        existing.AllDay = dto.AllDay;

        await _context.SaveChangesAsync();
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(string id)
    {
        var existing = await _context.CalendarEvents.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Event not found" });

        _context.CalendarEvents.Remove(existing);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
