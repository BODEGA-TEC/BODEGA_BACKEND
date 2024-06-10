using Microsoft.EntityFrameworkCore;
using Sibe.API.Models.Boletas;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Historicos;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            Asistente = Set<Asistente>();
            Categoria = Set<Categoria>();
            Componente = Set<Componente>();
            Boleta = Set<Boleta>();
            BoletaEquipo = Set<BoletaEquipo>();
            BoletaComponente = Set<BoletaComponente>();
            Equipo = Set<Equipo>();
            Estado = Set<Estado>();
            //HistoricoEquipo = Set<HistoricoEquipo>();
            //HistoricoComponente = Set<HistoricoComponente>();
            Solicitante = Set<Solicitante>();
            HistoricoRefreshToken = Set<HistoricoRefreshToken>();
            HistoricoClave = Set<HistoricoClave>();
            Usuario = Set<Usuario>();
        }

        public DbSet<Asistente> Asistente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Componente> Componente { get; set; }
        public DbSet<Boleta> Boleta { get; set; }
        public DbSet<BoletaEquipo> BoletaEquipo { get; set; }
        public DbSet<BoletaComponente> BoletaComponente { get; set; }
        public DbSet<Equipo> Equipo { get; set; }
        public DbSet<Estado> Estado { get; set; }
        //public DbSet<HistoricoEquipo> HistoricoEquipo { get; set; }
        //public DbSet<HistoricoComponente> HistoricoComponente { get; set; }
        public DbSet<Solicitante> Solicitante { get; set; }
        public DbSet<HistoricoRefreshToken> HistoricoRefreshToken { get; set; }
        public DbSet<HistoricoClave> HistoricoClave { get; set; }
        public DbSet<Usuario> Usuario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Atributos importantes de settear utilizando Fluent API
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

            // Configurar la relación entre Usuario y HistoricoClave
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.HistoricoClaves)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Categoria>()
                .HasIndex(c => new { c.Tipo, c.Nombre })
                .IsUnique();

            modelBuilder.Entity<Componente>()
                .HasIndex(c => c.ActivoBodega)
                .IsUnique();

            modelBuilder.Entity<Equipo>()
                .HasIndex(e => e.ActivoBodega)
                .IsUnique();

            // Configurar la relación entre Boleta y Equipo
            modelBuilder.Entity<BoletaEquipo>()
                .HasKey(be => new { be.BoletaId, be.EquipoId });

            modelBuilder.Entity<BoletaEquipo>()
                .HasOne(be => be.Boleta)
                .WithMany(b => b.BoletaEquipo)
                .HasForeignKey(be => be.BoletaId);

            modelBuilder.Entity<BoletaEquipo>()
                .HasOne(be => be.Equipo)
                .WithMany(e => e.BoletasEquipo)
                .HasForeignKey(be => be.EquipoId);

            // Configurar la relación entre Boleta y Componente
            modelBuilder.Entity<BoletaComponente>()
                .HasKey(bc => new { bc.BoletaId, bc.ComponenteId });

            modelBuilder.Entity<BoletaComponente>()
                .HasOne(bc => bc.Boleta)
                .WithMany(b => b.BoletaComponentes)
                .HasForeignKey(bc => bc.BoletaId);

            modelBuilder.Entity<BoletaComponente>()
                .HasOne(bc => bc.Componente)
                .WithMany(e => e.BoletasComponente)
                .HasForeignKey(bc => bc.ComponenteId);

            // Estados iniciales
            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = 1, Nombre = "DISPONIBLE", Descripcion = "El item está disponible en bodega y listo para ser prestado a los solicitantes." },
                new Estado { Id = 2, Nombre = "PRESTADO", Descripcion = "El item ha sido entregado a un solicitante y está a la espera de ser devuelto." },
                new Estado { Id = 3, Nombre = "AGOTADO", Descripcion = "No quedan componentes disponibles en bodega ya que todos han sido prestados." },
                new Estado { Id = 4, Nombre = "DAÑADO", Descripcion = "El item está dañado y no puede ser prestado en su estado actual." },
                new Estado { Id = 5, Nombre = "EN REPARACION", Descripcion = "El item se encuentra en mantenimiento y reparación." },
                new Estado { Id = 6, Nombre = "RETIRADO", Descripcion = "El item ha sido retirado de la bodega, donado o descontinuado su uso." },
                new Estado { Id = 7, Nombre = "APARTADO", Descripcion = "Un funcionario ha apartado el item y está reservado para su préstamo futuro." }
                          
            );
        }
    }
}
