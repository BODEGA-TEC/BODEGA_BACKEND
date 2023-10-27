using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models.Inventario;
using AutoMapper;

namespace Sibe.API
{
    public class AutoMapperProfiles : Profile
    {
        private string NullableString(string? sourceValue)
        {
            return sourceValue ?? string.Empty;
        }

        public AutoMapperProfiles()
        {
            CreateMap<Equipo, ReadEquipoDto>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => NullableString(src.Marca)))
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => NullableString(src.Modelo)))
                .ForMember(dest => dest.Estante, opt => opt.MapFrom(src => NullableString(src.Estante)))
                .ForMember(dest => dest.ActivoTec, opt => opt.MapFrom(src => NullableString(src.ActivoTec)))
                .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => NullableString(src.Serie)))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => NullableString(src.Observaciones)));
        }

    }
}
