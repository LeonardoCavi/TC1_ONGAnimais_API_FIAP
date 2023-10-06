using AutoMapper;
using ONGAnimaisAPI.Application.ViewModels.Autorizacao;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.API.Mappings
{
    public class AutenticaProfile : Profile
    {
        public AutenticaProfile()
        {
            CreateMap<AutenticaViewModel, Aplicacao>();
        }
    }
}