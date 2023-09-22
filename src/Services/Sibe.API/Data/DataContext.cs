using Microsoft.EntityFrameworkCore;
using Sibe.API.Models;
using Sibe.API.Models.Enums;

namespace Sibe.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Asistente" },
                new Rol { Id = 2, Nombre = "DesarrolladorWeb" },
                new Rol { Id = 3, Nombre = "Administrador" }
            );

            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = 1, Descripcion = "DISPONIBLE" },
                new Estado { Id = 2, Descripcion = "PRESTADO" },
                new Estado { Id = 3, Descripcion = "AGOTADO" },
                new Estado { Id = 4, Descripcion = "DAÑADO" },
                new Estado { Id = 5, Descripcion = "EN REPARACION" },
                new Estado { Id = 6, Descripcion = "RETIRADO" }
            );

            modelBuilder.Entity<Categoria>().HasData(
                // COMPONENTE
                new Categoria { Id = 1, Descripcion = "TTL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 2, Descripcion = "OPERACIONALES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 3, Descripcion = "RESISTENCIAS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 4, Descripcion = "POTENCIOMETROS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 5, Descripcion = "CAPACITORES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 6, Descripcion = "PRECISION", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 7, Descripcion = "CRISTALES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 8, Descripcion = "CMOS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 9, Descripcion = "BASES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 10, Descripcion = "TECLADO HEXADECIMAL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 11, Descripcion = "LCD", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 12, Descripcion = "OSCILOSCOPIO MINI", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 13, Descripcion = "CABLE WIRE WRAP", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 14, Descripcion = "PUERTO SERIAL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 15, Descripcion = "DISIPADOR", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 16, Descripcion = "SENSORES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 17, Descripcion = "TRANSISTORES/DIODOS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 18, Descripcion = "ADC/DAC", Tipo = TipoCategoria.COMPONENTE },

                // EQUIPO
                new Categoria { Id = 19, Descripcion = "MULTIMETRO", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 20, Descripcion = "GENERADOR FUNCIONES", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 21, Descripcion = "FUENTE", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 22, Descripcion = "MONITOR", Tipo = TipoCategoria.EQUIPO }
            );

            modelBuilder.Entity<Escuela>().HasData(
                new Escuela { Id = 1, Nombre = "ESCUELA DE ELETRONICA" }
            );

            //modelBuilder.Entity<Profesor>().HasData(
            //    new Profesor { Nombre = "Andrea", PrimerApellido = "Montoya", SegundoApellido = "Quesada", Email = "anmontoya@itcr.ac.cr" },
            //    new Profesor { Nombre = "Aníbal", PrimerApellido = "Coto", SegundoApellido = "Cortés", Email = "acotoc@itcr.ac.cr" },
            //    new Profesor { Nombre = "Anibal", PrimerApellido = "Ruiz", SegundoApellido = "Barquero", Email = "aniruiz@itcr.ac.cr" },
            //    new Profesor { Nombre = "Carlos Fabián", PrimerApellido = "Coto", SegundoApellido = "Calvo", Email = "fcoto@itcr.ac.cr" },
            //    new Profesor { Nombre = "Carlos Mauricio", PrimerApellido = "Segura", SegundoApellido = "Quiros", Email = "csegura@itcr.ac.cr" },
            //    new Profesor { Nombre = "Eduardo Interiano", PrimerApellido = "Interiano", SegundoApellido = "Salguero", Email = "einteriano@itcr.ac.cr" },
            //    new Profesor { Nombre = "Francisco", PrimerApellido = "Navarro", SegundoApellido = "Henriquez", Email = "fnavarro@itcr.ac.cr" },
            //    new Profesor { Nombre = "Hayden Anthony", PrimerApellido = "Phillips", SegundoApellido = "Brenes", Email = "hphillips@itcr.ac.cr" },
            //    new Profesor { Nombre = "Hugo", PrimerApellido = "Sánchez", SegundoApellido = "Ortíz", Email = "husanchez@itcr.ac.cr" },
            //    new Profesor { Nombre = "Javier", PrimerApellido = "Pérez", SegundoApellido = "Rodríguez", Email = "japerez@itcr.ac.cr" },
            //    new Profesor { Nombre = "Johan", PrimerApellido = "Carvajal", SegundoApellido = "Godínez", Email = "johcarvajal@itcr.ac.cr" },
            //    new Profesor { Nombre = "Jorge", PrimerApellido = "Castro", SegundoApellido = "Godinez", Email = "jocastro@itcr.ac.cr" },
            //    new Profesor { Nombre = "José Alberto", PrimerApellido = "Díaz", SegundoApellido = "García", Email = "jdiaz@itcr.ac.cr" },
            //    new Profesor { Nombre = "José Miguel", PrimerApellido = "Barboza", SegundoApellido = "Retana", Email = "jmbarboza@itcr.ac.cr" },
            //    new Profesor { Nombre = "Juan Carlos", PrimerApellido = "Jiménez", SegundoApellido = "Robles", Email = "juanjimenez@itcr.ac.cr" },
            //    new Profesor { Nombre = "Juan Jose", PrimerApellido = "Montero", SegundoApellido = "Rodriguez", Email = "jjmontero@itcr.ac.cr" },
            //    new Profesor { Nombre = "Juan Scott", PrimerApellido = "Chaves", SegundoApellido = "Noguera", Email = "jschaves@itcr.ac.cr" },
            //    new Profesor { Nombre = "Laura Cristina", PrimerApellido = "Cabrera", SegundoApellido = "Quiros", Email = "lcabrera@itcr.ac.cr" },
            //    new Profesor { Nombre = "Leonardo", PrimerApellido = "Sandoval", SegundoApellido = "Cascante", Email = "lesandoval@itcr.ac.cr" },
            //    new Profesor { Nombre = "Marvin", PrimerApellido = "Hernández", SegundoApellido = "Cisneros", Email = "marhernandez@itcr.ac.cr" },
            //    new Profesor { Nombre = "Miguel Angel", PrimerApellido = "Hernandez", SegundoApellido = "Rivera", Email = "mhernandez@itcr.ac.cr" },
            //    new Profesor { Nombre = "Nestor", PrimerApellido = "Hernandez", SegundoApellido = "Hostaller", Email = "nhernandez@itcr.ac.cr" },
            //    new Profesor { Nombre = "Adolfo", PrimerApellido = "Chaves", SegundoApellido = "Jiménez", Email = "adchaves@itcr.ac.cr" },
            //    new Profesor { Nombre = "Pablo", PrimerApellido = "Alvarado", SegundoApellido = "Moya", Email = "palvarado@itcr.ac.cr" },
            //    new Profesor { Nombre = "Renato", PrimerApellido = "Rimolo", SegundoApellido = "Donadío", Email = "rrimolo@itcr.ac.cr" },
            //    new Profesor { Nombre = "Roberto Carlos", PrimerApellido = "Molina", SegundoApellido = "Robles", Email = "rmolina@itcr.ac.cr" },
            //    new Profesor { Nombre = "Ronald", PrimerApellido = "Soto", SegundoApellido = "Fallas", Email = "rsoto@itcr.ac.cr" },
            //    new Profesor { Nombre = "Ronny", PrimerApellido = "Garcia", SegundoApellido = "Ramirez", Email = "rgarcia@itcr.ac.cr" },
            //    new Profesor { Nombre = "Sergio", PrimerApellido = "Arriola", SegundoApellido = "Valverde", Email = "sarriola@itcr.ac.cr" },
            //    new Profesor { Nombre = "Sergio Arturo", PrimerApellido = "Morales", SegundoApellido = "Hernández", Email = "smorales@itcr.ac.cr" },
            //    new Profesor { Nombre = "William", PrimerApellido = "Marín", SegundoApellido = "Moreno", Email = "wmarin@itcr.ac.cr" },
            //    new Profesor { Nombre = "William", PrimerApellido = "Quiros", SegundoApellido = "Solano", Email = "wquiros@itcr.ac.cr" }
            //);
        }

        public DbSet<Estado> Estado { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Equipo> Equipo { get; set; }
        public DbSet<Componente> Componente { get; set; }
    }
}
