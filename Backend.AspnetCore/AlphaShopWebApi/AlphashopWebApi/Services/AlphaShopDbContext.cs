using AlphashopWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlphashopWebApi.Services
{
    public class AlphaShopDbContext : DbContext
    {
        public AlphaShopDbContext(DbContextOptions<AlphaShopDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Articoli> Articoli { get; set; }
        public virtual DbSet<Ean> Barcode { get; set; }
        public virtual DbSet<FamilyAssort> Famassort { get; set; }
        public virtual DbSet<Ingredienti> Ingredienti { get; set; }
        public virtual DbSet<Iva> Iva { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setto la chiave primaria
            modelBuilder.Entity<Articoli>().HasKey(a => new { a.CodArt });

            // Relazione uno a molti fra articoli e barcode
            modelBuilder.Entity<Ean>()
            .HasOne(s => s.Articolo) //ad un articolo
            .WithMany(g => g.Barcode) //corrispondono molti barcode
            .HasForeignKey(s => s.CodArt); //la chiave esterna dell'entity barcode

            // Relazione uno a uno fra articoli e ingredienti
            modelBuilder.Entity<Articoli>()
            .HasOne(s => s.ingredienti) //ad un ingrediente
            .WithOne(g => g.Articolo) //corrisponde un articolo
            .HasForeignKey<Ingredienti>(s => s.CodArt);

            // Relazione uno a molti fra iva e articoli
            modelBuilder.Entity<Articoli>()
            .HasOne<Iva>(s => s.iva)
            .WithMany(g => g.Articoli)
            .HasForeignKey(s => s.IdIva);

            // Relazione uno a molti fra FamAssort e Articoli
            modelBuilder.Entity<Articoli>()
            .HasOne<FamilyAssort>(s => s.familyAssort)
            .WithMany(g => g.Articoli)
            .HasForeignKey(s => s.IdFamAss);
        }
    }
}

