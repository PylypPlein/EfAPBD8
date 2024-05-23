using EfAPBD8.Models;
using Microsoft.EntityFrameworkCore;

namespace EfAPBD8.Context;

public class ApbdContext : DbContext
{
    public ApbdContext()
    {
        
    }

    public ApbdContext(DbContextOptions<ApbdContext> options) : base(options)
    {
        
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<ClientTrip> ClientTrips { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<CountryTrip> CountryTrips { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(
                "Data Source=localhost;Initial Catalog=APBD;User Id=sa;Password=YourStrongPassw0rd;Encrypt=False")
            .LogTo(Console.WriteLine, LogLevel.Information);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient).HasName("Client_pk");
                entity.ToTable("Client", "trip");
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(120);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(120);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(120);
                entity.Property(e => e.Telephone).IsRequired().HasMaxLength(120);
                entity.Property(e => e.Pesel).IsRequired().HasMaxLength(120);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.IdTrip).HasName("Trip_pk");
                entity.ToTable("Trip", "trip");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(120);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(220);
                entity.Property(e => e.DateFrom).IsRequired();
                entity.Property(e => e.DateTo).IsRequired();
                entity.Property(e => e.MaxPeople).IsRequired();
            });

            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");
                entity.ToTable("Client_Trip", "trip");
                entity.Property(e => e.RegisteredAt).IsRequired();
                entity.Property(e => e.PaymentDate);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("Table_5_Client");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .HasConstraintName("Table_5_Trip");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry).HasName("Country_pk");
                entity.ToTable("Country", "trip");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(120);
            });

            modelBuilder.Entity<CountryTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdCountry, e.IdTrip }).HasName("Country_Trip_pk");
                entity.ToTable("Country_Trip", "trip");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryTrips)
                    .HasForeignKey(d => d.IdCountry)
                    .HasConstraintName("Country_Trip_Country");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.CountryTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .HasConstraintName("Country_Trip_Trip");
            });
        }
}