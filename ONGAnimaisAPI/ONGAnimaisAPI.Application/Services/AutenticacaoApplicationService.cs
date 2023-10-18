using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.Validations.Autorizacao;
using ONGAnimaisAPI.Application.Validations.ONG;
using ONGAnimaisAPI.Application.ViewModels.Autorizacao;
using ONGAnimaisAPI.Domain.Abstracts;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Services;

namespace ONGAnimaisAPI.Application.Services
{
    public class AutenticacaoApplicationService : NotificadorContext, IAutenticacaoApplicationService
    {
        private readonly IAplicacaoService _service;
        private readonly IMapper _mapper;

        public AutenticacaoApplicationService(IAplicacaoService service,
                                              IMapper mapper,
                                              INotificador notificador) : base(notificador)
        {
            this._service = service;
            this._mapper = mapper;
        }

        public async Task<object> Autenticar(AutenticaViewModel aut)
        {
            ExecutarValidacao(new AutenticaValidation(), aut);

            if (!_notificador.TemNotificacao())
            {
                var autMap = _mapper.Map<Aplicacao>(aut);
                return await _service.Autenticar(autMap.Usuario, autMap.Senha);
            }

            return null;
        }
    }
}