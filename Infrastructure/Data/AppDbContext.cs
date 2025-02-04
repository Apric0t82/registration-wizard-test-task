using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AppUser>()
                .HasOne(e => e.Country)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<AppUser>()
                .HasOne(e => e.Province)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Countries
            builder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Canada" },
                new Country { Id = 2, Name = "Spain" }
            );

            // Seed Provinces
            builder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Alberta", CountryId = 1 },
                new Province { Id = 2, Name = "British Columbia", CountryId = 1 },
                new Province { Id = 3, Name = "Manitoba", CountryId = 1 },
                new Province { Id = 4, Name = "Barcelona", CountryId = 2 },
                new Province { Id = 5, Name = "Granada", CountryId = 2 },
                new Province { Id = 6, Name = "Malaga", CountryId = 2 }
            );
        }
    }
}
