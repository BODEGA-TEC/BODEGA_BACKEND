using Sibe.API.Data.Dtos.Equipo;
using Sibe.API.Models.Inventario;
using AutoMapper;

namespace Sibe.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Equipo, ReadEquipoDto>()
            .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nombre))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre));
        }
    }
}
