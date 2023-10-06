using AutoMapper;
using ONGAnimaisAPI.Domain.Notifications;
using ONGAnimaisAPI.Application.ViewModels;
using System.Net;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Evento;

namespace ONGAnimaisAPI.API.Mappings
{
    public class ErroProfile: Profile
    {
        public ErroProfile()
        {
            CreateMap<IEnumerable<Notificacao>, ErroViewModel>()
                .ForMember(dest => dest.Erros,
                    map => map.MapFrom(src => src.Select(x => x.Mensagem)))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom<ErroMappingStatusCode>());

            CreateMap<Exception, ErroViewModel>()
                .ForMember(dest => dest.Erros,
                    map => map.MapFrom(src => new List<string>() { src.Message }))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom(src => 500));
        }
    }

    public class ErroMappingStatusCode : IValueResolver<IEnumerable<Notificacao>, ErroViewModel, int>
    {
        public int Resolve(IEnumerable<Notificacao> source, ErroViewModel destination, int destMember, ResolutionContext context)
        {
            var notificacao = source.FirstOrDefault();

            switch (notificacao.Tipo)
            {
                case TipoNotificacao.Validation:
                    return (int) HttpStatusCode.BadRequest;
                case TipoNotificacao.NotFound:
                    return (int) HttpStatusCode.NotFound;
                case TipoNotificacao.Conflict:
                    return (int)HttpStatusCode.Conflict;
                default:
                    return (int) HttpStatusCode.InternalServerError;
            }
        }
    }
}
