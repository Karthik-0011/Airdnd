using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Airdnd.Models.DomainModels;

namespace Airdnd.Models.DataLayer.Configuration
{
    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> entity)
        {
            entity.HasData(
                new Location { LocationId = "chicago", Name = "Chicago" },
                new Location { LocationId = "atlanta", Name = "Atlanta" },
                new Location { LocationId = "miami", Name = "Miami" },
                new Location { LocationId = "nyc", Name = "New York" }
            );
        }
    }
}