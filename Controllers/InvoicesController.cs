using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rasket.Api.Data;
using Rasket.Api.DTOs;

namespace Rasket.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoicesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<InvoiceDto>>> GetInvoices([FromQuery] PagedRequest request)
    {
        var query = _context.Invoices.Include(i => i.Client).AsNoTracking().AsQueryable();

        // Search term matching multiple fields
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(i =>
                i.InvoiceNumber.ToLower().Contains(term) ||
                (i.Client != null && i.Client.Name.ToLower().Contains(term)) ||
                i.Status.ToLower().Contains(term) ||
                i.PaymentMethod.ToLower().Contains(term)
            );
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(request.SortColumn))
        {
            var isDesc = string.Equals(request.SortDirection, "desc", System.StringComparison.OrdinalIgnoreCase);
            query = request.SortColumn.ToLower() switch
            {
                "amount" => isDesc ? query.OrderByDescending(i => i.Amount) : query.OrderBy(i => i.Amount),
                "invoicenumber" => isDesc ? query.OrderByDescending(i => i.InvoiceNumber) : query.OrderBy(i => i.InvoiceNumber),
                "status" => isDesc ? query.OrderByDescending(i => i.Status) : query.OrderBy(i => i.Status),
                _ => isDesc ? query.OrderByDescending(i => i.IssueDate) : query.OrderBy(i => i.IssueDate)
            };
        }
        else
        {
            query = query.OrderByDescending(i => i.IssueDate);
        }

        var total = await query.CountAsync();
        var page = request.Page > 0 ? request.Page : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize : 10;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InvoiceDto
            {
                InvoiceNumber = i.InvoiceNumber,
                Client = new ClientDto
                {
                    Name = i.Client != null ? i.Client.Name : "Unknown Client",
                    Avatar = i.Client != null ? i.Client.Avatar : "assets/images/users/avatar-1.jpg"
                },
                IssueDate = i.IssueDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                DueDate = i.DueDate.ToString("yyyy-MM-dd"),
                Amount = i.Amount,
                Status = i.Status,
                PaymentMethod = i.PaymentMethod
            })
            .ToListAsync();

        return Ok(new PagedResult<InvoiceDto>
        {
            Items = items,
            Total = total
        });
    }

    [HttpGet("{invoiceNumber}")]
    public async Task<ActionResult<InvoiceDetailDto>> GetInvoiceDetails(string invoiceNumber)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Client)
            .Include(i => i.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);

        if (invoice == null)
            return NotFound(new { message = $"Invoice {invoiceNumber} not found" });

        var products = invoice.Items.Select(item => new InvoiceItemDto
        {
            Name = item.Name,
            Qty = item.Qty,
            Price = item.Price
        }).ToList();

        var subTotal = products.Sum(p => p.Qty * p.Price);
        var discount = subTotal * 0.10m;
        var totalAmount = subTotal - discount;

        return Ok(new InvoiceDetailDto
        {
            InvoiceNumber = invoice.InvoiceNumber,
            Client = new ClientDto
            {
                Name = invoice.Client?.Name ?? "Unknown Client",
                Avatar = invoice.Client?.Avatar ?? "assets/images/users/avatar-1.jpg"
            },
            IssueDate = invoice.IssueDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            DueDate = invoice.DueDate.ToString("yyyy-MM-dd"),
            Amount = invoice.Amount,
            Status = invoice.Status,
            PaymentMethod = invoice.PaymentMethod,
            Products = products,
            SubTotal = subTotal,
            DiscountPrice = discount,
            TotalAmount = totalAmount
        });
    }
}
