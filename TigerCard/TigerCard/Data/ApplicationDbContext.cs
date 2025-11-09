using Microsoft.EntityFrameworkCore;
using TigerCard.Models;

namespace TigerCard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TarjetaCredito> Tarjetas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<TarjetaCredito>()
                .HasIndex(t => t.Numero)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Tarjetas)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId);
        }
    }
}
