namespace EfAPBD8.Models;

public class ClientTrip
{
    public int IdClient { get; set; }
    
    public int IdTrip { get; set; }
    
    public int RegisteredAt { get; set; }
    
    public int PaymentDate { get; set; }
    
    //public virtual Client IdClientNavigation { get; set; } = null!;

    //public virtual Trip IdTripNavigation { get; set; } = null!;
    public Trip Trip { get; set; }
    public Client Client { get; set; }
}