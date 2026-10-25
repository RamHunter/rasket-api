using System;
using Microsoft.EntityFrameworkCore;
using Rasket.Api.Models;

namespace Rasket.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
    public DbSet<TodoItem> Todos => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Client)
            .WithMany()
            .HasForeignKey(i => i.ClientId);

        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Items)
            .WithOne()
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Default Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "user",
                Email = "user@demo.com",
                PasswordHash = "123456", // In production, hash with BCrypt / ASP.NET Identity
                FirstName = "user",
                LastName = "demo",
                Avatar = "assets/images/users/user-1.jpg",
                Location = "California, USA",
                Title = "Admin Head",
                Role = "Admin"
            }
        );

        // Seed Clients
        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "Sean Kemper", Avatar = "assets/images/users/avatar-2.jpg", Email = "sean@example.com" },
            new Client { Id = 2, Name = "Victoria Sullivan", Avatar = "assets/images/users/avatar-3.jpg", Email = "victoria@example.com" },
            new Client { Id = 3, Name = "Liam Martinez", Avatar = "assets/images/users/avatar-4.jpg", Email = "liam@example.com" },
            new Client { Id = 4, Name = "Emma Johnson", Avatar = "assets/images/users/avatar-5.jpg", Email = "emma@example.com" },
            new Client { Id = 5, Name = "Olivia Thompson", Avatar = "assets/images/users/avatar-1.jpg", Email = "olivia@example.com" }
        );

        // Seed Invoices
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice { Id = 1, InvoiceNumber = "RB6985", ClientId = 1, IssueDate = new DateTime(2026, 4, 23, 17, 9, 0), DueDate = new DateTime(2026, 4, 30), Amount = 852.25m, Status = "Unpaid", PaymentMethod = "PayPal" },
            new Invoice { Id = 2, InvoiceNumber = "RB1002", ClientId = 2, IssueDate = new DateTime(2026, 5, 14, 10, 51, 0), DueDate = new DateTime(2026, 8, 25), Amount = 953.00m, Status = "Send", PaymentMethod = "PayPal" },
            new Invoice { Id = 3, InvoiceNumber = "RB3652", ClientId = 3, IssueDate = new DateTime(2026, 4, 12, 12, 9, 0), DueDate = new DateTime(2026, 4, 28), Amount = 99.00m, Status = "Unpaid", PaymentMethod = "Swift Transfer" },
            new Invoice { Id = 4, InvoiceNumber = "RB7854", ClientId = 4, IssueDate = new DateTime(2026, 4, 10, 22, 9, 0), DueDate = new DateTime(2026, 4, 15), Amount = 1250.00m, Status = "Paid", PaymentMethod = "PayPal" },
            new Invoice { Id = 5, InvoiceNumber = "RB9521", ClientId = 5, IssueDate = new DateTime(2026, 5, 22, 15, 41, 0), DueDate = new DateTime(2026, 7, 5), Amount = 500.00m, Status = "Send", PaymentMethod = "Payoneer" }
        );

        // Seed Invoice Items
        modelBuilder.Entity<InvoiceItem>().HasData(
            new InvoiceItem { Id = 1, InvoiceId = 1, Name = "G15 Gaming Laptop", Qty = 3, Price = 240.59m },
            new InvoiceItem { Id = 2, InvoiceId = 1, Name = "Sony Alpha ILCE 6000Y 24.3 MP Mirrorless Digital SLR Camera", Qty = 5, Price = 135.99m },
            new InvoiceItem { Id = 3, InvoiceId = 1, Name = "Sony Over-Ear Wireless Headphone with Mic", Qty = 1, Price = 99.49m }
        );

        // Seed Calendar Events
        modelBuilder.Entity<CalendarEvent>().HasData(
            new CalendarEvent { Id = "1", Title = "Interview - Backend Engineer", Start = DateTime.UtcNow, ClassName = "bg-primary" },
            new CalendarEvent { Id = "2", Title = "Meeting with CT Team", Start = DateTime.UtcNow.AddHours(4), ClassName = "bg-warning" },
            new CalendarEvent { Id = "3", Title = "Interview - Frontend Engineer", Start = DateTime.UtcNow.AddDays(1), End = DateTime.UtcNow.AddDays(1).AddHours(2), ClassName = "bg-secondary" },
            new CalendarEvent { Id = "4", Title = "Setup Production API", Start = DateTime.UtcNow.AddDays(2), ClassName = "bg-success" }
        );

        // Seed Todos
        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem { Id = 1, TaskName = "Review system logs for any reported errors", CreateDate = "23 April, 2026", Time = "05:09 PM", DueDate = "30 April, 2026", AssignedName = "Sean Kemper", AssignedAvatar = "assets/images/users/avatar-2.jpg", Status = "In-progress", Priority = "High", Checked = false },
            new TodoItem { Id = 2, TaskName = "Conduct user testing to identify potential bugs", CreateDate = "14 May, 2026", Time = "10:51 AM", DueDate = "25 Aug, 2026", AssignedName = "Victoria Sullivan", AssignedAvatar = "assets/images/users/avatar-3.jpg", Status = "Pending", Priority = "Low", Checked = true },
            new TodoItem { Id = 3, TaskName = "Prioritize bugs based on severity and impact", CreateDate = "10 April, 2026", Time = "10:09 PM", DueDate = "15 April, 2026", AssignedName = "Emma Johnson", AssignedAvatar = "assets/images/users/avatar-5.jpg", Status = "Completed", Priority = "Medium", Checked = false }
        );
    }
}
