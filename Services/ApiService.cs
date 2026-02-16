using System.Net.Http.Headers;
using DataSync.Data;
using DataSync.DTOs;
using DataSync.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSync.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;

    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient, AppDbContext context, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _context = context;
        _configuration = configuration;
    }

    public async Task SyncDataAsync()
    {
        // 1️⃣ LOGIN
        var loginRequest = new LoginRequest
        {
            Username = _configuration["ApiSettings:Username"] ?? string.Empty,
            Password = _configuration["ApiSettings:Password"] ?? string.Empty
        };

        var loginResponse = await _httpClient.PostAsJsonAsync(
            "api/Account/Login", loginRequest);

        loginResponse.EnsureSuccessStatusCode();

        var token = await loginResponse.Content.ReadFromJsonAsync<string>();

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // 2️⃣ GET PLATFORM DATA
        var response = await _httpClient.GetFromJsonAsync<List<PlatformDto>>(
            "api/PlatformWell/GetPlatformWellActual");
            // "api/PlatformWell/GetPlatformWellDummy");

        if (response == null)
            return;

        foreach (var platformDto in response)
        {
            var existingPlatform = await _context.Platforms
                .Include(p => p.Wells)
                .FirstOrDefaultAsync(p => p.ExternalId == platformDto.Id);

            if (existingPlatform == null)
            {
                var newPlatform = new Platform
                {
                    ExternalId = platformDto.Id,
                    UniqueName = platformDto.UniqueName,
                    Latitude = platformDto.Latitude,
                    Longitude = platformDto.Longitude,
                    CreatedAt = platformDto.CreatedAt,
                    UpdatedAt = platformDto.UpdatedAt
                };

                foreach (var wellDto in platformDto.Well)
                {
                    newPlatform.Wells.Add(new Well
                    {
                        ExternalId = wellDto.Id,
                        UniqueName = wellDto.UniqueName,
                        Latitude = wellDto.Latitude,
                        Longitude = wellDto.Longitude,
                        CreatedAt = wellDto.CreatedAt,
                        UpdatedAt = wellDto.UpdatedAt
                    });
                }

                _context.Platforms.Add(newPlatform);
            }
            else
            {
                // Update platform
                existingPlatform.UniqueName = platformDto.UniqueName;
                existingPlatform.Latitude = platformDto.Latitude;
                existingPlatform.Longitude = platformDto.Longitude;
                existingPlatform.UpdatedAt = platformDto.UpdatedAt;

                // Sync wells
                foreach (var wellDto in platformDto.Well)
                {
                    var existingWell = existingPlatform.Wells
                        .FirstOrDefault(w => w.ExternalId == wellDto.Id);

                    if (existingWell == null)
                    {
                        existingPlatform.Wells.Add(new Well
                        {
                            ExternalId = wellDto.Id,
                            UniqueName = wellDto.UniqueName,
                            Latitude = wellDto.Latitude,
                            Longitude = wellDto.Longitude,
                            CreatedAt = wellDto.CreatedAt,
                            UpdatedAt = wellDto.UpdatedAt
                        });
                    }
                    else
                    {
                        existingWell.UniqueName = wellDto.UniqueName;
                        existingWell.Latitude = wellDto.Latitude;
                        existingWell.Longitude = wellDto.Longitude;
                        existingWell.UpdatedAt = wellDto.UpdatedAt;
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }

}
