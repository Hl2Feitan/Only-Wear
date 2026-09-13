using Microsoft.EntityFrameworkCore;
using OnlyWear.Api.Entities;

namespace OnlyWear.Api.Data;

public class OnlyWearDbContext : DbContext
{
    public OnlyWearDbContext(DbContextOptions<OnlyWearDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<PrecioPorTalle> PreciosPorTalle => Set<PrecioPorTalle>();
    public DbSet<DisenoPersonalizado> DisenosPersonalizados => Set<DisenoPersonalizado>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.Property(p => p.Total)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<PrecioPorTalle>(entity =>
        {
            entity.Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(18,2)");

            entity.ToTable(t => t.HasCheckConstraint(
                "CK_DetallePedido_ProductoODiseno",
                "(\"ProductoId\" IS NOT NULL AND \"DisenoPersonalizadoId\" IS NULL) OR (\"ProductoId\" IS NULL AND \"DisenoPersonalizadoId\" IS NOT NULL)"));
        });

        modelBuilder.Entity<DisenoPersonalizado>(entity =>
        {
            entity.HasOne(d => d.ProductoBase)
                .WithMany(p => p.DisenosBasados)
                .HasForeignKey(d => d.ProductoBaseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}