using ONGAnimaisAPI.Domain.Abstracts;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Repositories;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using ONGAnimaisAPI.Domain.Notifications;

namespace ONGAnimaisAPI.Domain.Services
{
    public class AplicacaoService : NotificadorContext, IAplicacaoService
    {
        private readonly IAplicacaoRepository _aRepository;
        private readonly ITokenService _tService;

        public AplicacaoService(IAplicacaoRepository aRepository,
                                INotificador notificador,
                                ITokenService tService) : base(notificador)
        {
            this._aRepository = aRepository;
            this._tService = tService;
        }

        public async Task<string> Autenticar(string usuario, string senha)
        {
            var aut = await _aRepository.Autenticar(usuario, senha);

            if (aut == null)
            {
                Notificar($"Autenticar: usuário e/ou senha incorretos", TipoNotificacao.Unauthorized);
                return null;
            }
            else
            {
               return _tService.GerarToken(usuario);
            }
        }
    }
}