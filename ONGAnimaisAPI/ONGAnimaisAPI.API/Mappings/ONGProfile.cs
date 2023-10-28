using AutoMapper;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.API.Mappings
{
    public class ONGProfile : Profile
    {
        public ONGProfile()
        {
            CreateMap<InsereONGViewModel, ONG>()
                .AfterMap((src, dest) => dest.GeoLocalizacao = new());

            CreateMap<AtualizaONGViewModel, ONG>()
                .AfterMap((src, dest) => dest.GeoLocalizacao = new()); ;

            CreateMap<ONG, ObtemONGViewModel>();

            CreateMap<ONG, ObtemONGEventosViewModel>();

            CreateMap<ONG, ObtemONGGeoViewModel>();

            CreateMap<InsereEventoViewModel, Evento>()
                .AfterMap((src, dest) => dest.GeoLocalizacao = new()); ;

            CreateMap<AtualizaEventoViewModel, Evento>()
                .AfterMap((src, dest) => dest.GeoLocalizacao = new()); ;

            CreateMap<Evento, ObtemEventoViewModel>();

            CreateMap<Evento, ObtemEventoGeoViewModel>();
        }
    }
}
