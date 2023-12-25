using System.Text.Json.Serialization;

namespace Sibe.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BoletaEstado
    {
        PENDIENTE = 0,
        CERRADO = 1,
    }

    /* 
     *    Pendiente: indica que está en un estado inicial o no ha progresado.
     *    Cerrado: indica que el proceso ha sido completado.
     */
}
