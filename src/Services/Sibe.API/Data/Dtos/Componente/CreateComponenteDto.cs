﻿namespace Sibe.API.Data.Dtos.Componente
{
    public class CreateComponenteDto
    {
        public string Activo { get; set; } = string.Empty!;

        public int CategoriaId { get; set; }

        public string Descripcion { get; set; } = string.Empty!;

        public string Observaciones { get; set; } = string.Empty;
    }
}
