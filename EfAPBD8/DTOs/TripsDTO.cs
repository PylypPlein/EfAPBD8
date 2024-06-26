﻿namespace EfAPBD8.DTOs;

public class TripsDTO
{
    public class TripDTO
    {
        public int IdTrip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<ClientDTO> Clients { get; set; }
        public List<CountryDTO> Countries { get; set; }
    }
    public class ClientDTO
    {
        public int IdClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
    }

    public class CountryDTO
    {
        public int IdCountry { get; set; }
        public string Name { get; set; }
    }
}