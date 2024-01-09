﻿using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Data.Dtos.Usuario
{
    public class LoginUsuarioDto
    {
        public string Username { get; set; } = string.Empty!;
        public string Clave { get; set; } = string.Empty!;
    }
}
