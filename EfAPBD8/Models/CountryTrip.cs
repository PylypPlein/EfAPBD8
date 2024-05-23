namespace EfAPBD8.Models;

public class CountryTrip
{
    public int IdCountry { get; set; }

    public int IdTrip { get; set; }
    
    public virtual Country IdCountryNavigation { get; set; } = null!;

    public virtual Trip IdTripNavigation { get; set; } = null!;

}