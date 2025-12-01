using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Airdnd.Models.DomainModels;

namespace Airdnd.Models.DataLayer.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasData(
                new User { UserId = 1, Name = "Admin Owner", Email = "owner@airdnd.com", DOB = new DateTime(1990, 1, 1), UserType = "Owner" },
                new User { UserId = 2, Name = "Test Client", Email = "client@test.com", DOB = new DateTime(1995, 5, 5), UserType = "Client" }
            );
        }
    }
}