using EfAPBD8.Context;
using EfAPBD8.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfAPBD8.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly ApbdContext _context;

    public TripController(ApbdContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTripsAsync()
    {
        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.Client)
            .Include(t => t.CountryTrips)
            .ThenInclude(ct => ct.Country)
            .OrderByDescending(t => t.DateFrom)
            .Select(t => new TripsDTO.TripDTO
            {
                IdTrip = t.IdTrip,
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new TripsDTO.CountryDTO
                {
                    Name = ct.Country.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new TripsDTO.ClientDTO
                {
                    FirstName = ct.Client.FirstName,
                    LastName = ct.Client.LastName,
                }).ToList()
                
            })
            .ToListAsync();

        return Ok(trips);
    }
}