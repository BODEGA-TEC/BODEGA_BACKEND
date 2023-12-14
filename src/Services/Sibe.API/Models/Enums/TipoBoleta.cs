using System.Text.Json.Serialization;

namespace Sibe.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TipoBoleta
    {
        PRESTAMO = 1,
        DEVOLUCION = 2,
    }
}