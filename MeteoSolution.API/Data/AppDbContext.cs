using Microsoft.EntityFrameworkCore;
using MeteoSolution.API.Models;

namespace MeteoSolution.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<Pais> Paises { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<RegiaoMonitorada> RegioesMonitoradas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RegiaoMonitorada>(entity =>
        {
            entity.ToTable("regiao_monitorada");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TipoSolo).HasMaxLength(100);
            entity.Property(e => e.NivelUrbanizacao).HasMaxLength(100);

            entity.HasOne(e => e.Cidade)
                  .WithMany(c => c.RegioesMonitoradas)
                  .HasForeignKey(e => e.CidadeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.ToTable("cidade");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);

            entity.HasOne(e => e.Estado)
                  .WithMany(e => e.Cidades)
                  .HasForeignKey(e => e.EstadoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("estado");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Sigla).IsRequired().HasMaxLength(2);

            entity.HasOne(e => e.Pais)
                  .WithMany(p => p.Estados)
                  .HasForeignKey(e => e.PaisId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.ToTable("pais");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CodigoIso).IsRequired().HasMaxLength(3);
        });
    }
}