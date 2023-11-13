using System.Text.Json.Serialization;

namespace Sibe.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoActivo
    {
        DESCONOCIDO = 0,
        EQUIPO = 1,
        COMPONENTE = 2,
    }
}