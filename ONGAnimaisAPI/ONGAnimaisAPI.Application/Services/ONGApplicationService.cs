using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.ViewModels.Evento;
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

        #region [ONG]

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

        public async Task<ObtemONGEventosViewModel> ObterONGEventos(int id)
        {
            var ongevento = await _service.ObterONGEventos(id);
            return _mapper.Map<ObtemONGEventosViewModel>(ongevento);
        }

        public async Task<ICollection<ObtemONGViewModel>> ObterTodasONG()
        {
            var ongs = await _service.ObterTodasONG();
            return _mapper.Map<List<ObtemONGViewModel>>(ongs);
        }

        #endregion [ONG]

        #region [Usuario]

        public async Task InserirEvento(int ongId, InsereEventoViewModel evento)
        {
            var eventoMap = _mapper.Map<Evento>(evento);
            eventoMap.OngId = ongId;

            await _service.InserirEvento(eventoMap);
        }

        public async Task AtualizarEvento(int ongId, AtualizaEventoViewModel evento)
        {
            var eventoMap = _mapper.Map<Evento>(evento);
            eventoMap.OngId = ongId;

            await _service.AtualizarEvento(eventoMap);
        }

        public async Task<ObtemEventoViewModel> ObterEvento(int ongId, int id)
        {
            var evento = await _service.ObterEvento(ongId, id);
            return _mapper.Map<ObtemEventoViewModel>(evento);
        }

        public async Task ExcluirEvento(int ongId, int id)
        {
            await _service.ExcluirEvento(ongId, id);
        }

        #endregion [Usuario]
    }
}