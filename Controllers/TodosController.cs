using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rasket.Api.Data;
using Rasket.Api.DTOs;
using Rasket.Api.Models;

namespace Rasket.Api.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TodoDto>>> GetTodos([FromQuery] PagedRequest request)
    {
        var query = _context.Todos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(t => t.TaskName.ToLower().Contains(term) || t.AssignedName.ToLower().Contains(term) || t.Status.ToLower().Contains(term));
        }

        var total = await query.CountAsync();
        var page = request.Page > 0 ? request.Page : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize : 10;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TodoDto
            {
                Id = t.Id,
                TaskName = t.TaskName,
                CreateDate = t.CreateDate,
                Time = t.Time,
                DueDate = t.DueDate,
                Assigned = new ClientDto { Name = t.AssignedName, Avatar = t.AssignedAvatar },
                Status = t.Status,
                Priority = t.Priority,
                Checked = t.Checked
            })
            .ToListAsync();

        return Ok(new PagedResult<TodoDto>
        {
            Items = items,
            Total = total
        });
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleChecked(long id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
            return NotFound();

        todo.Checked = !todo.Checked;
        await _context.SaveChangesAsync();
        return Ok(new { id = todo.Id, @checked = todo.Checked });
    }
}
