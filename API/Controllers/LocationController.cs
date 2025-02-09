using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers;

public class LocationController(AppDbContext context) : BaseApiController
{
    private readonly AppDbContext _context = context;

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _context.Countries.AsNoTracking()
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        return Ok(countries);
    }

    [HttpGet("provinces/{countryId}")]
    public async Task<IActionResult> GetProvinces(int countryId)
    {
        var provinces = await _context.Provinces.AsNoTracking()
            .Where(p => p.CountryId == countryId)
            .Select(p => new { p.Id, p.Name })
            .ToListAsync();

        if (provinces.Count == 0)
            return NotFound($"No provinces found for country with ID {countryId}");

        return Ok(provinces);
    }
}
