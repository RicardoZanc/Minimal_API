using Bandas_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bandas_Api.Data
{
    public class MinimalContextDb : DbContext
    {
        public MinimalContextDb(DbContextOptions options) : base(options) { }

        public DbSet<Bandas> Bandas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //id, name, genre, active

            modelBuilder.Entity<Bandas>().HasKey(p => p.Id);

            modelBuilder.Entity<Bandas>()
                .Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Bandas>()
                .Property(p => p.Genre)
                .IsRequired()
                .HasColumnType("varchar(25)");

            modelBuilder.Entity<Bandas>()
                .ToTable("Bandas");

            base.OnModelCreating(modelBuilder);

        }
    }
}
