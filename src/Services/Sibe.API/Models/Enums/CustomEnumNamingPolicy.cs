using System.Text.Json;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Enums;


/* Usar los valores personalizados de [Display] cuando estén disponibles 
 * y, en caso contrario, aplicar la estrategia de nomenclatura que 
 * agrega espacios antes de las mayúsculas:
 */
public class CustomEnumNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var fieldInfo = typeof(TipoEstado).GetField(name);
        if (fieldInfo != null)
        {
            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
            {
                return displayAttribute.Name;
            }
        }

        // Si no se encontró un atributo [Display], simplemente usa el nombre tal como está
        return name;

        //// Aplicar la estrategia de nomenclatura personalizada (agregar espacios antes de las mayúsculas)
        //var sb = new StringBuilder();
        //foreach (char c in name)
        //{
        //    if (char.IsUpper(c) && sb.Length > 0)
        //    {
        //        sb.Append(' '); // Agrega un espacio antes de cada mayúscula, excepto al principio
        //    }
        //    sb.Append(c);
        //}
        //return sb.ToString();
    }
}


