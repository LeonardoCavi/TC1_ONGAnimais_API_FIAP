using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Services;

namespace ONGAnimaisAPI.Application.Services
{
    public class ONGApplicationService : IONGApplicationService
    {
        private readonly IONGService _service;
        private readonly IMapper _mapper;

        public ONGApplicationService(IONGService service,
                                     IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }

        public async Task AtualizarONG(AtualizaONGViewModel ong)
        {
            var ongMap = _mapper.Map<ONG>(ong);
            await _service.AtualizarONG(ongMap);
        }

        public async Task ExcluirONG(int id)
        {
            await _service.ExcluirONG(id);
        }

        public async Task InserirONG(InsereONGViewModel ong)
        {
            var ongMap = _mapper.Map<ONG>(ong);
            await _service.InserirONG(ongMap);
        }

        public async Task<ObtemONGViewModel> ObterONG(int id)
        {
            var ong = await _service.ObterONG(id);
            return _mapper.Map<ObtemONGViewModel>(ong);
        }

        public async Task<ICollection<ObtemONGViewModel>> ObterTodasONG()
        {
            var ongs = await _service.ObterTodasONG();
            return _mapper.Map<List<ObtemONGViewModel>>(ongs);
        }
    }
}