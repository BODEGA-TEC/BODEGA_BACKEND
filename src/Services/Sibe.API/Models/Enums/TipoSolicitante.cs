using System.Text.Json.Serialization;

namespace Sibe.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoSolicitante
    {
        ESTUDIANTE = 1,
        PROFESOR = 2,
    }
}
