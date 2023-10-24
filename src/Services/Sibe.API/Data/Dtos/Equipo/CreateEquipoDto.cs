﻿using Sibe.API.Models.Enums;

namespace Sibe.API.Data.Dtos.Equipo
{
    public class CreateEquipoDto
    {

        public int CategoriaId { get; set; }

        public int EstadoId { get; set; } = 1;

        public string Descripcion { get; set; } = string.Empty!;

        public Condicion Condicion { get; set; }

        public string Estante { get; set; } = string.Empty!;

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string? ActivoTec { get; set; }

        public string? Serie { get; set; }

        public string? Observaciones { get; set; }
    }
}
