using ONGAnimaisAPI.Domain.Abstracts;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using ONGAnimaisAPI.Domain.Notifications;

namespace ONGAnimaisAPI.Domain.Services
{
    public class ONGService : NotificadorContext, IONGService
    {
        private readonly IONGRepository _oRepository;
        private readonly IEventoRepository _eRepository;

        public ONGService(
            IONGRepository oRepository,
            IEventoRepository eRepository,
            INotificador notificador) : base(notificador)
        {
            this._oRepository = oRepository;
            this._eRepository = eRepository;
        }

        #region [Evento]
        public async Task AtualizarEvento(Evento evento)
        {
            var ongDB = await ObterONG(evento.Id);
            if(ongDB != null)
            {
                var eventoDB = await _eRepository.Obter(evento.Id);
                if (eventoDB != null)
                {
                    eventoDB.Nome = evento.Nome;
                    eventoDB.Descricao = evento.Descricao;
                    eventoDB.Endereco = evento.Endereco;
                    eventoDB.Data = evento.Data;

                    await _eRepository.Atualizar(eventoDB);
                }
            }
        }

        public async Task ExcluirEvento(int ongId, int id)
        {
            var ongDB = await ObterONG(ongId);

            if (ongDB == null)
            {
                Notificar($"ONG: ong com id '{id}' não existe", TipoNotificacao.NotFound);
            }
            else
            {
                var evento = await _eRepository.Obter(id);

                if (evento == null)
                {
                    Notificar($"Evento: evento com id '{id}' não existe", TipoNotificacao.NotFound);
                }
                else
                {
                    await _eRepository.Excluir(evento);
                }
            }
        }

        public async Task InserirEvento(Evento evento)
        {
            var ongDB = await ObterONG(evento.OngId);

            if (ongDB != null)
            {
                await _eRepository.Inserir(evento);
            }
        }

        public async Task<Evento> ObterEvento(int ongId, int id)
        {
            var ongDB = await ObterONG(ongId);

            if (ongDB == null)
            {
                Notificar($"ONG: ong com id '{id}' não existe", TipoNotificacao.NotFound);
                return null;
            }
            else
            {
                var evento = await _eRepository.Obter(id);

                if(evento == null)
                {
                    Notificar($"Evento: evento com id '{id}' não existe", TipoNotificacao.NotFound);
                    return null;
                }
                else
                {
                    return evento;
                }
            }
        }

        public async Task<ICollection<Evento>> ObterTodosEventos()
        {
            var eventos = await _eRepository.ObterTodos();

            if (eventos == null)
            {
                Notificar($"Evento: não existe eventos cadastrados", TipoNotificacao.NotFound);
            }

            return eventos;
        }
        #endregion

        #region [ONG]

        public async Task AtualizarONG(ONG ong)
        {
            var ongDB = await _oRepository.Obter(ong.Id);
            if (ongDB != null)
            {
                ongDB.Nome = ong.Nome;
                ongDB.Descricao = ong.Descricao;
                ongDB.Responsavel = ong.Responsavel;
                ongDB.Email = ong.Email;
                ongDB.Endereco = ong.Endereco;
                ongDB.Telefones = ong.Telefones;
                ongDB.Contatos = ong.Contatos;

                await _oRepository.Atualizar(ongDB);
            }
        }

        public async Task ExcluirONG(int id)
        {
            var ong = await _oRepository.Obter(id);

            if (ong == null)
            {
                Notificar($"ONG: ong com id '{id}' não existe", TipoNotificacao.NotFound);
            }
            else
            {
                await _oRepository.Excluir(ong);
            }
        }

        public async Task InserirONG(ONG ong)
        {
            await _oRepository.Inserir(ong);
        }

        public async Task<ONG> ObterONG(int id)
        {
            var ong =  await _oRepository.Obter(id);

            if(ong == null)
            {
                Notificar($"ONG: ong com id '{id}' não existe", TipoNotificacao.NotFound);
            }

            return ong;
        }

        public async Task<ICollection<ONG>> ObterTodasONG()
        {
            var ongs =  await _oRepository.ObterTodos();

            if (ongs == null)
            {
                Notificar($"ONG: não existe ongs cadastradas", TipoNotificacao.NotFound);
            }

            return ongs;
        }

        public async Task<ONG> ObterONGEventos(int id)
        {
            var ong =  await _oRepository.ObterONGEventos(id);

            if (ong == null)
            {
                Notificar($"ONG: ong com id '{id}' não existe", TipoNotificacao.NotFound);
            }

            return ong;
        }

        #endregion
    }
}