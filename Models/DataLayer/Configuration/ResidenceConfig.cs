using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Airdnd.Models.DomainModels;

namespace Airdnd.Models.DataLayer.Configuration
{
    public class ResidenceConfig : IEntityTypeConfiguration<Residence>
    {
        public void Configure(EntityTypeBuilder<Residence> entity)
        {
            entity.HasData(
                new Residence { ResidenceId = 1, Name = "Chicago Loop Apartment", ResidencePicture = "chicago_apt.jpg", LocationId = "chicago", GuestNumber = 4, BedroomNumber = 2, BathroomNumber = 2, PricePerNight = 250.00, BuiltYear = 2010, UserId = 1 },
                new Residence { ResidenceId = 2, Name = "Atlanta Suburban House", ResidencePicture = "atlanta_house.jpg", LocationId = "atlanta", GuestNumber = 8, BedroomNumber = 4, BathroomNumber = 3, PricePerNight = 320.00, BuiltYear = 1995, UserId = 1 },
                new Residence { ResidenceId = 3, Name = "Miami Beach House", ResidencePicture = "miami_house.jpg", LocationId = "miami", GuestNumber = 10, BedroomNumber = 5, BathroomNumber = 4.5, PricePerNight = 700.00, BuiltYear = 2018, UserId = 1 },
                new Residence { ResidenceId = 4, Name = "NYC Studio", ResidencePicture = "nyc_studio.jpg", LocationId = "nyc", GuestNumber = 2, BedroomNumber = 1, BathroomNumber = 1, PricePerNight = 180.00, BuiltYear = 1980, UserId = 1 }
            );
        }
    }
}