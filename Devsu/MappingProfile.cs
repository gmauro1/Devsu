using AutoMapper;

namespace Devsu
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Cliente, DTO.Cliente>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.PersonaNavigation.Nombre))
                .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.PersonaNavigation.Genero))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.PersonaNavigation.Edad))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.PersonaNavigation.Identificacion))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.PersonaNavigation.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.PersonaNavigation.Telefono));

            CreateMap<DTO.Cliente, Entities.Persona>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<DTO.Cliente, Entities.Cliente>();

            CreateMap<Entities.Cuenta, DTO.Cuenta>();
            CreateMap<DTO.Cuenta, Entities.Cuenta>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<Entities.Movimiento, DTO.MovimientoResponse>();
            CreateMap<DTO.Movimiento, Entities.Movimiento>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Fecha, opt => opt.Ignore())
                .ForMember(c => c.TipoMovimiento, opt => opt.Ignore());
        }
    }
}
