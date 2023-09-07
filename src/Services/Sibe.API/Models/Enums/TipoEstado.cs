using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Sibe.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TipoEstado
    {
        [Display(Name = "En Bodega")]
        BODEGA = 1,

        [Display(Name = "En Mantenimiento")]
        MANTENIMIENTO = 2,

        [Display(Name = "Agotado")]
        AGOTADO = 3
    }
}