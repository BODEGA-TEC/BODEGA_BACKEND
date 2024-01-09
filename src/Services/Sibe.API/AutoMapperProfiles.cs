using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models.Inventario;
using AutoMapper;
using Sibe.API.Data.Dtos.Componente;
using Sibe.API.Data.Dtos.Boletas;
using Sibe.API.Models.Boletas;
using Sibe.API.Models.Entidades;
using Sibe.API.Data.Dtos.Asistente;

namespace Sibe.API
{
    public class AutoMapperProfiles : Profile
    {
        private static string NullableString(string? sourceValue)
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

            CreateMap<Componente, ReadComponenteDto>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.NoParte, opt => opt.MapFrom(src => NullableString(src.NoParte)))
                .ForMember(dest => dest.Estante, opt => opt.MapFrom(src => NullableString(src.Estante)))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => NullableString(src.Observaciones)));

            CreateMap<Boleta, ReadBoletaDto>();
            CreateMap<Asistente, ReadAsistenteDto>();

        }
    }

}
