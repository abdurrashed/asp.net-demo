using System.Collections.Generic;
using System.Reflection.Emit;
using ASP.NET.DEMO.API.Models.DTOS.SystemConfig.AddressBook;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.DEMO.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DTOAddressBook> AddressBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DTOAddressBook>(entity =>
            {
                entity.HasKey(e => e.address_id);
                entity.Property(e => e.address_id).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

