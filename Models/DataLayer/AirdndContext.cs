using Microsoft.EntityFrameworkCore;
using Airdnd.Models.DomainModels;
using Airdnd.Models.DataLayer.Configuration;

namespace Airdnd.Models.DataLayer
{
    public class AirdndContext : DbContext
    {
        public AirdndContext(DbContextOptions<AirdndContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Residence> Residences { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LocationConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new ResidenceConfig());
        }
    }
}