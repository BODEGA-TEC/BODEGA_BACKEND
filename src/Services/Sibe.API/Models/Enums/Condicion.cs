using System.Text.Json.Serialization;

namespace Sibe.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum Condicion
    {
        DAÑADO = -1,
        REGULAR = 0,
        BUENO = 1
    }
}