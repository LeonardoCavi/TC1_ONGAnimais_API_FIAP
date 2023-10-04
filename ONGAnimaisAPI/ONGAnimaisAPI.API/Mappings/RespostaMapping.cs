﻿using AutoMapper;
using ONGAnimaisAPI.Domain.Notifications;
using ONGAnimaisAPI.Application.ViewModels;
using System.Net;
using ONGAnimaisAPI.Application.ViewModels.Usuario;

namespace ONGAnimaisAPI.API.Mappings
{
    public class RespostaMapping: Profile
    {
        public RespostaMapping()
        {
            CreateMap<IEnumerable<Notificacao>, RespostaViewModel<object>>()
                .ForMember(dest => dest.Sucesso,
                    map => map.MapFrom(src => false))
                .ForMember(dest => dest.Erros,
                    map => map.MapFrom(src => src.Select(x => x.Mensagem)))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom<RespostaMappingStatusCode>());

            CreateMap<InsereUsuarioViewModel, RespostaViewModel<InsereUsuarioViewModel>>()
                .ForMember(dest => dest.Sucesso,
                    map => map.MapFrom(src => true))
                .ForMember(dest => dest.Objeto,
                    map => map.MapFrom(src => src))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom(src => 201));

            CreateMap<ObtemUsuarioViewModel, RespostaViewModel<ObtemUsuarioViewModel>>()
                .ForMember(dest => dest.Sucesso,
                    map => map.MapFrom(src => true))
                .ForMember(dest => dest.Objeto,
                    map => map.MapFrom(src => src))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom(src => 200));

            CreateMap<List<ObtemUsuarioViewModel>, RespostaViewModel<List<ObtemUsuarioViewModel>>>()
                .ForMember(dest => dest.Sucesso,
                    map => map.MapFrom(src => true))
                .ForMember(dest => dest.Objeto,
                    map => map.MapFrom(src => src))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom(src => 200));

            CreateMap<ObtemUsuarioEventosViewModel, RespostaViewModel<ObtemUsuarioEventosViewModel>>()
                .ForMember(dest => dest.Sucesso,
                    map => map.MapFrom(src => true))
                .ForMember(dest => dest.Objeto,
                    map => map.MapFrom(src => src))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom(src => 200));

            CreateMap<ObtemUsuarioONGsViewModel, RespostaViewModel<ObtemUsuarioONGsViewModel>>()
                .ForMember(dest => dest.Sucesso,
                    map => map.MapFrom(src => true))
                .ForMember(dest => dest.Objeto,
                    map => map.MapFrom(src => src))
                .ForMember(dest => dest.StatusCode,
                    map => map.MapFrom(src => 200));

        }
    }

    public class RespostaMappingStatusCode : IValueResolver<IEnumerable<Notificacao>, RespostaViewModel<object>, int>
    {
        public int Resolve(IEnumerable<Notificacao> source, RespostaViewModel<object> destination, int destMember, ResolutionContext context)
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
