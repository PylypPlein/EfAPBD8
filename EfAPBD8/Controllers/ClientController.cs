using EfAPBD8.Context;

namespace EfAPBD8.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly ApbdContext _context;

    public ClientController(ApbdContext context)
    {
        _context = context;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        
        var hasTrips = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == id);
        if (hasTrips)
        {
            return BadRequest("Nie można usunąć klienta, który ma przypisane wycieczki :)");
        }
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}