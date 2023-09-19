using ONGAnimaisAPI.Domain.Interfaces;

namespace ONGAnimaisAPI.Domain.Services
{
    public class ONGService
    {
        private readonly IONGRepository _oRepository;
        private readonly IEventoRepository _eRepository;

        public ONGService(IONGRepository oRepository,
                          IEventoRepository eRepository)
        {
            this._oRepository = oRepository;
            this._eRepository = eRepository;
        }
    }
}