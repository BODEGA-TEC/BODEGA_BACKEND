using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        /* PROPIEDADES DE NAVEGACION */

        [Required]
        public Categoria Categoria { get; set; } = null!;

        public Estado Estado { get; set; } = null!;

        /* PROPIEDADES DE PROPIAS */

        [Required]
        public string Descripcion { get; set; } = string.Empty!;

        [Required]
        public string ActivoBodega { get; set; } = null!;

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string? ActivoTec { get; set; }

        public string? Serie { get; set; }

        public string? Observaciones { get; set; }
    }
}
