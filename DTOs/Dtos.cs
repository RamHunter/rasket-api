using System;
using System.Collections.Generic;

namespace Rasket.Api.DTOs;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Location { get; set; }
    public string? Title { get; set; }
    public string Token { get; set; } = string.Empty;
}

public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string? SortDirection { get; set; } = "asc";
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int Total { get; set; }
}

public class ClientDto
{
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
}

public class InvoiceDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public ClientDto Client { get; set; } = new();
    public string IssueDate { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
}

public class InvoiceDetailDto : InvoiceDto
{
    public List<InvoiceItemDto> Products { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal TotalAmount { get; set; }
}

public class InvoiceItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}

public class CalendarEventDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string ClassName { get; set; } = "bg-primary";
    public bool AllDay { get; set; }
}

public class TodoDto
{
    public long Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public ClientDto Assigned { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool Checked { get; set; }
}
