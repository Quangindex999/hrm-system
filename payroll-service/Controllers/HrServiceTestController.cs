using Microsoft.AspNetCore.Mvc;
using payroll_service.Services;
using System.Text.Json;

namespace payroll_service.Controllers;

[ApiController]
[Route("api/test")]
public class HrServiceTestController : ControllerBase
{
    private readonly IHrServiceClient _hrServiceClient;

    public HrServiceTestController(IHrServiceClient hrServiceClient)
    {
        _hrServiceClient = hrServiceClient;
    }

    [HttpGet("hr-employees")]
    public async Task<IActionResult> GetEmployees()
    {
        var result = await _hrServiceClient.GetEmployeesAsync();

        // Try to parse JSON string into object; if fails, return raw string
        try
        {
            var doc = JsonSerializer.Deserialize<JsonElement>(result);
            return Ok(new { success = true, count = (doc.ValueKind == JsonValueKind.Array ? doc.GetArrayLength() : 1), data = doc });
        }
        catch
        {
            return Ok(new { success = true, data = result });
        }
    }
}
