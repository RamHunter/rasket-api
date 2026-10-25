using System;
using System.Collections.Generic;

namespace Rasket.Api.Models;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Location { get; set; }
    public string? Title { get; set; }
    public string Role { get; set; } = "Admin";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CalendarEvent
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string ClassName { get; set; } = "bg-primary";
    public bool AllDay { get; set; } = false;
    public long? UserId { get; set; }
}

public class Client
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string? Email { get; set; }
}

public class Invoice
{
    public long Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public long ClientId { get; set; }
    public Client? Client { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Unpaid"; // Paid, Unpaid, Send
    public string PaymentMethod { get; set; } = "PayPal";
    public List<InvoiceItem> Items { get; set; } = new();
}

public class InvoiceItem
{
    public long Id { get; set; }
    public long InvoiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}

public class TodoItem
{
    public long Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public string AssignedName { get; set; } = string.Empty;
    public string AssignedAvatar { get; set; } = string.Empty;
    public string Status { get; set; } = "New"; // In-progress, Pending, Completed, New
    public string Priority { get; set; } = "Medium"; // High, Medium, Low
    public bool Checked { get; set; } = false;
}
