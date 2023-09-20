using AutoMapper;
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

            CreateMap<List<ONG>, List<ObtemONGViewModel>>();
        }
    }
}
