namespace Sibe.API.Models.Entidades
{
    public class Solicitante
    {
        private string _nombre = string.Empty;

        public string Carne { get; set; } = string.Empty;

        public string Nombre
        {
            get => _nombre;
            set => _nombre = value?.ToUpperInvariant() ?? string.Empty;
        }

        public string Correo { get; set; } = string.Empty;
    }

}
