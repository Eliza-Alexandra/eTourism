using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTourismWebApp.Data;

public class eTourismWebAppContext : IdentityDbContext<IdentityUser>
{
    public eTourismWebAppContext(DbContextOptions<eTourismWebAppContext> options)
        : base(options)
    {
    }

    public DbSet<Obiectiv> Obiective { get; set; } = default!;

    public DbSet<CategorieObiectiv> CategoriiObiective { get; set; } = default!;
    
    public DbSet<Cazare> Cazari { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Obiectiv>(entity =>
        {
            entity.ToTable("obiective");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).HasColumnName("id_obiectiv");
            entity.Property(o => o.Denumire).HasColumnName("denumire");
            entity.Property(o => o.Adresa).HasColumnName("adresa");
            entity.Property(o => o.Detalii).HasColumnName("detalii");
            entity.Property(o => o.Imagine).HasColumnName("imagine");
            entity.Property(o => o.IdCategorie).HasColumnName("id_categorie");

            entity.HasOne(o => o.Categorie)
                .WithMany()
                .HasForeignKey(o => o.IdCategorie);
        });

        builder.Entity<CategorieObiectiv>(entity =>
        {
            entity.ToTable("categorii_obiective");
            entity.HasKey(c => c.IdCategorie);
            entity.Property(c => c.IdCategorie).HasColumnName("id_categorie");
            entity.Property(c => c.NumeCategorie).HasColumnName("nume_categorie");
        });
        
        builder.Entity<Cazare>(entity =>
        {
            entity.ToTable("cazari");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).HasColumnName("id_cazare");
            entity.Property(o => o.Nume).HasColumnName("nume");
            entity.Property(o => o.Adresa).HasColumnName("adresa");
            entity.Property(o => o.Descriere).HasColumnName("descriere");
            entity.Property(o => o.Imagine).HasColumnName("imagine");
        });

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
