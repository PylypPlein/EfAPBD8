using EfAPBD8.Context;
using EfAPBD8.DTOs;
using EfAPBD8.Models;
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
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTripAsync(int idTrip, [FromBody] TripsDTO.ClientDTO clientData)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound();
        }
        
        var client = new Client
        {
            FirstName = clientData.FirstName,
            LastName = clientData.LastName,
            Email = clientData.Email,
            Telephone = clientData.Telephone,
            Pesel = clientData.Pesel
        };
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        
        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = 1,
            PaymentDate = 1
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return Ok("Klient został przypisany do wycieczki.");
    }
}