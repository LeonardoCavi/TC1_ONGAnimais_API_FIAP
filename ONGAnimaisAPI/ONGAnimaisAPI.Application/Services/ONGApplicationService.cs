using AutoMapper;
using ONGAnimaisAPI.Application.Interfaces;
using ONGAnimaisAPI.Application.Validations;
using ONGAnimaisAPI.Application.Validations.Evento;
using ONGAnimaisAPI.Application.Validations.ONG;
using ONGAnimaisAPI.Application.Validations.Usuario;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Domain.Abstracts;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Services;

namespace ONGAnimaisAPI.Application.Services
{
    public class ONGApplicationService : NotificadorContext, IONGApplicationService
    {
        private readonly IONGService _service;
        private readonly IMapper _mapper;

        public ONGApplicationService(IONGService service,
                                     IMapper mapper,
                                     INotificador notificador) : base(notificador)
        {
            this._service = service;
            this._mapper = mapper;
        }

        #region [ONG]

        public async Task AtualizarONG(AtualizaONGViewModel ong)
        {
            ExecutarValidacao(new AtualizaONGValidation(), ong);

            if (!_notificador.TemNotificacao())
            {
                var ongMap = _mapper.Map<ONG>(ong);
                await _service.AtualizarONG(ongMap);
            }
        }

        public async Task ExcluirONG(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (!_notificador.TemNotificacao())
            {
                await _service.ExcluirONG(id);
            }
        }

        public async Task InserirONG(InsereONGViewModel ong)
        {
            ExecutarValidacao(new InsereONGValidation(), ong);

            if (!_notificador.TemNotificacao())
            {
                var ongMap = _mapper.Map<ONG>(ong);
                await _service.InserirONG(ongMap);
            }
        }

        public async Task<ObtemONGViewModel> ObterONG(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (_notificador.TemNotificacao())
                return null;

            var ong = await _service.ObterONG(id);

            if (_notificador.TemNotificacao())
                return null;

            return _mapper.Map<ObtemONGViewModel>(ong);
        }

        public async Task<ObtemONGEventosViewModel> ObterONGEventos(int id)
        {
            ExecutarValidacao(new IdValidation(), id);

            if (_notificador.TemNotificacao())
                return null;

            var ongevento = await _service.ObterONGEventos(id);

            if (_notificador.TemNotificacao())
                return null;

            return _mapper.Map<ObtemONGEventosViewModel>(ongevento);
        }

        public async Task<ICollection<ObtemONGViewModel>> ObterTodasONG()
        {
            var ongs = await _service.ObterTodasONG();

            if (_notificador.TemNotificacao())
                return null;

            return _mapper.Map<List<ObtemONGViewModel>>(ongs);
        }

        public async Task<ICollection<ObtemONGViewModel>> ObterONGsPorCidade(BuscaONGCidadeViewModel ongcidade, int paginacao = 0)
        {
            ExecutarValidacao(new BuscaONGCidadeValidation(), ongcidade);

            if (!_notificador.TemNotificacao())
            {
                var ongs = await _service.ObterONGsPorCidade(ongcidade.Cidade, ongcidade.UF, paginacao);

                return _mapper.Map<ICollection<ObtemONGViewModel>>(ongs);
            }

            return null;
        }

        public async Task<ICollection<ObtemONGGeoViewModel>> ObterONGsPorCidadeGeo(decimal latitude, decimal longitude, int paginacao = 0)
        {
            ExecutarValidacao(new LatitudeLongitudeValidation(), (latitude, longitude));

            if (!_notificador.TemNotificacao())
            {
                var ongs = await _service.ObterONGsPorCidadeGeo(latitude, longitude, paginacao);

                return _mapper.Map<ICollection<ObtemONGGeoViewModel>>(ongs);
            }

            return null;
        }

        #endregion [ONG]

        #region [Evento]

        public async Task InserirEvento(int ongId, InsereEventoViewModel evento)
        {
            ExecutarValidacao(new IdValidation(), ongId);

            if (!_notificador.TemNotificacao())
            {
                ExecutarValidacao(new InsereEventoValidation(), evento);

                if (!_notificador.TemNotificacao())
                {
                    var eventoMap = _mapper.Map<Evento>(evento);
                    eventoMap.OngId = ongId;

                    await _service.InserirEvento(eventoMap);
                }
            }
        }

        public async Task AtualizarEvento(int ongId, AtualizaEventoViewModel evento)
        {
            ExecutarValidacao(new IdValidation(), ongId);

            if (!_notificador.TemNotificacao())
            {
                ExecutarValidacao(new AtualizaEventoValidation(), evento);

                if (!_notificador.TemNotificacao())
                {
                    var eventoMap = _mapper.Map<Evento>(evento);
                    eventoMap.OngId = ongId;

                    await _service.AtualizarEvento(eventoMap);
                }
            }
        }

        public async Task<ObtemEventoViewModel> ObterEvento(int ongId, int id)
        {
            ExecutarValidacao(new IdEventoValidation(), (ongId, id));

            if (!_notificador.TemNotificacao())
            {
                var evento = await _service.ObterEvento(ongId, id);
                if (!_notificador.TemNotificacao())
                {
                    return _mapper.Map<ObtemEventoViewModel>(evento);
                }
            }

            return null;
        }

        public async Task ExcluirEvento(int ongId, int id)
        {
            ExecutarValidacao(new IdEventoValidation(), (ongId, id));

            if (!_notificador.TemNotificacao())
            {
                await _service.ExcluirEvento(ongId, id);
            }
        }

        public async Task<ICollection<ObtemEventoViewModel>> ObterEventosPorCidade(BuscaEventoCidadeViewModel eventocidade, int paginacao = 0)
        {
            ExecutarValidacao(new BuscaEventoCidadeValidation(), eventocidade);

            if (!_notificador.TemNotificacao())
            {
                var eventos = await _service.ObterEventosPorCidade(eventocidade.Cidade, eventocidade.UF, paginacao);

                return _mapper.Map<ICollection<ObtemEventoViewModel>>(eventos);
            }

            return null;
        }

        public async Task<ICollection<ObtemEventoGeoViewModel>> ObterEventosPorCidadeGeo(decimal latitude, decimal longitude, int paginacao = 0)
        {
            ExecutarValidacao(new LatitudeLongitudeValidation(), (latitude, longitude));

            if (!_notificador.TemNotificacao())
            {
                var eventos = await _service.ObterEventosPorCidadeGeo(latitude, longitude, paginacao);

                return _mapper.Map<ICollection<ObtemEventoGeoViewModel>>(eventos);
            }

            return null;
        }

        #endregion [Evento]
    }
}