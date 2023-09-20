using ONGAnimaisAPI.Domain.Interfaces.Repository;

namespace ONGAnimaisAPI.Domain.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _uRepository;
        private readonly IONGRepository _oRepository;
        private readonly IEventoRepository _eRepository;

        public UsuarioService(IONGRepository oRepository,
                              IEventoRepository eRepository,
                              IUsuarioRepository uRepository)
        {
            this._oRepository = oRepository;
            this._eRepository = eRepository;
            this._uRepository = uRepository;
        }
    }
}