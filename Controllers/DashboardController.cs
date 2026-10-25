using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Rasket.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        return Ok(new[]
        {
            new { title = "Page View", icon = "solar:leaf-bold-duotone", stat = "13, 647", change = "2.3%", variant = "success" },
            new { title = "Clicks", icon = "solar:cpu-bolt-line-duotone", stat = "9, 526", change = "8.1%", variant = "success" },
            new { title = "Conversions", icon = "solar:layers-bold-duotone", stat = "976", change = "0.3%", variant = "danger" },
            new { title = "New Users", icon = "solar:users-group-two-rounded-bold-duotone", stat = "$123.6k", change = "10.6%", variant = "danger" }
        });
    }

    [HttpGet("browsers")]
    public IActionResult GetBrowsers()
    {
        return Ok(new[]
        {
            new { name = "Chrome", percentage = 62.5, amount = 5.06 },
            new { name = "Firefox", percentage = 12.3, amount = 1.50 },
            new { name = "Safari", percentage = 9.86, amount = 1.03 },
            new { name = "Brave", percentage = 3.15, amount = 0.30 },
            new { name = "Opera", percentage = 3.01, amount = 1.58 }
        });
    }

    [HttpGet("countries")]
    public IActionResult GetCountries()
    {
        return Ok(new[]
        {
            new { icon = "circle-flags:us", name = "United States", progressValue = 82.5, progressAmount = 659, progressType = "secondary" },
            new { icon = "circle-flags:ru", name = "Russia", progressValue = 70.5, progressAmount = 485, progressType = "info" },
            new { icon = "circle-flags:cn", name = "China", progressValue = 65.8, progressAmount = 355, progressType = "warning" },
            new { icon = "circle-flags:ca", name = "Canada", progressValue = 55.8, progressAmount = 204, progressType = "success" }
        });
    }
}
