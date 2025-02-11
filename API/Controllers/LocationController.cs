using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class LocationController(AppDbContext context) : BaseApiController
{
    private readonly AppDbContext _context = context;

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries(CancellationToken token)
    {
        var countries = await _context.Countries
            .Select(c => new { c.Id, c.Name })
            .ToListAsync(token);

        return Ok(countries);
    }

    [HttpGet("provinces/{countryId}")]
    public async Task<IActionResult> GetProvinces(int countryId, CancellationToken token)
    {
        var provinces = await _context.Provinces
            .Where(p => p.CountryId == countryId)
            .Select(p => new { p.Id, p.Name })
            .ToListAsync(token);

        if (provinces.Count == 0)
            return NotFound($"No provinces found for country with ID {countryId}");

        return Ok(provinces);
    }
}
