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
            CreateMap<InsereONGViewModel, ONG>();

            CreateMap<AtualizaONGViewModel, ONG>();

            CreateMap<ONG, ObtemONGViewModel>();

            CreateMap<ONG, ObtemONGEventosViewModel>();

            CreateMap<InsereEventoViewModel, Evento>();

            CreateMap<AtualizaEventoViewModel, Evento>();

            CreateMap<Evento, ObtemEventoViewModel>();
        }
    }
}
