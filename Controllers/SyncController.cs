using DataSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataSync.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly IApiService _apiService;

    public SyncController(IApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpPost]
    public async Task<IActionResult> Sync()
    {
        await _apiService.SyncDataAsync();
        return Ok("Sync completed successfully.");
    }
}
