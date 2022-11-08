using AlphashopWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlphashopWebApi.Services
{
    public class AlphaShopDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public AlphaShopDbContext(
            DbContextOptions<AlphaShopDbContext> options,
            IConfiguration configuration
        ) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Articoli> Articoli => Set<Articoli>();
        public virtual DbSet<Ean> Barcode => Set<Ean>();
        public virtual DbSet<FamilyAssort> Famassort => Set<FamilyAssort>();
        public virtual DbSet<Ingredienti> Ingredienti => Set<Ingredienti>();
        public virtual DbSet<Iva> Iva => Set<Iva>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to mysql with connection string from app settings
            var connectionString = _configuration.GetConnectionString("alphashopDbConString");
            options.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setto la chiave primaria
            modelBuilder.Entity<Articoli>().HasKey(a => new { a.CodArt });

            // Relazione uno a molti fra articoli e barcode
            modelBuilder
                .Entity<Ean>()
                .HasOne(s => s.Articolo) //ad un articolo
                .WithMany(g => g.Barcode) //corrispondono molti barcode
                .HasForeignKey(s => s.CodArt); //la chiave esterna dell'entity barcode

            // Relazione uno a uno fra articoli e ingredienti
            modelBuilder
                .Entity<Articoli>()
                .HasOne(s => s.ingredienti) //ad un ingrediente
                .WithOne(g => g.Articolo) //corrisponde un articolo
                .HasForeignKey<Ingredienti>(s => s.CodArt);

            // Relazione uno a molti fra iva e articoli
            modelBuilder
                .Entity<Articoli>()
                .HasOne<Iva>(s => s.iva)
                .WithMany(g => g.Articoli)
                .HasForeignKey(s => s.IdIva);

            // Relazione uno a molti fra FamAssort e Articoli
            modelBuilder
                .Entity<Articoli>()
                .HasOne<FamilyAssort>(s => s.familyAssort)
                .WithMany(g => g.Articoli)
                .HasForeignKey(s => s.IdFamAss);
        }
    }
}
