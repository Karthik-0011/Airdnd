using Microsoft.EntityFrameworkCore;

namespace Airdnd.Models
{
    public class AirdndContext : DbContext
    {
        public AirdndContext(DbContextOptions<AirdndContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Residence> Residences { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>().HasData(
                new Location { LocationId = "chicago", Name = "Chicago" },
                new Location { LocationId = "atlanta", Name = "Atlanta" },
                new Location { LocationId = "miami", Name = "Miami" },
                new Location { LocationId = "nyc", Name = "New York" }
            );

            modelBuilder.Entity<Residence>().HasData(
                 new Residence { ResidenceId = 1, Name = "Chicago Loop Apartment", ResidencePicture = "chicago_apt.jpg", LocationId = "chicago", GuestNumber = 4, BedroomNumber = 2, BathroomNumber = 2, PricePerNight = 250.00 },
                 new Residence { ResidenceId = 2, Name = "Atlanta Suburban House", ResidencePicture = "atlanta_house.jpg", LocationId = "atlanta", GuestNumber = 8, BedroomNumber = 4, BathroomNumber = 3, PricePerNight = 320.00 },
                 new Residence { ResidenceId = 3, Name = "Miami Beach House", ResidencePicture = "miami_house.jpg", LocationId = "miami", GuestNumber = 10, BedroomNumber = 5, BathroomNumber = 4, PricePerNight = 700.00 },
                 new Residence { ResidenceId = 4, Name = "NYC Studio", ResidencePicture = "nyc_studio.jpg", LocationId = "nyc", GuestNumber = 2, BedroomNumber = 1, BathroomNumber = 1, PricePerNight = 180.00 }
            );
        }
    }
}