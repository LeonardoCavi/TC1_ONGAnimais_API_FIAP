using AutoMapper;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.API.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<InsereUsuarioViewModel, Usuario>();

            CreateMap<AtualizaUsuarioViewModel, Usuario>();

            CreateMap<Usuario, ObtemUsuarioViewModel>();

            CreateMap<Usuario, ObtemUsuarioEventosViewModel>();

            CreateMap<Usuario, ObtemUsuarioONGsViewModel>();

            CreateMap<Usuario, ObtemUsuarioTelegramViewModel>();
        }
    }
}
