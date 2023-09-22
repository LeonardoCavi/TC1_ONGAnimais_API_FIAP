using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;

namespace ONGAnimaisAPI.Domain.Services
{
    public class ONGService : IONGService
    {
        private readonly IONGRepository _oRepository;
        private readonly IEventoRepository _eRepository;

        public ONGService(IONGRepository oRepository,
                          IEventoRepository eRepository)
        {
            this._oRepository = oRepository;
            this._eRepository = eRepository;
        }

        #region [Evento]
        public async Task AtualizarEvento(Evento evento)
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

        public async Task ExcluirEvento(int id)
        {
            var evento = await _eRepository.Obter(id);
            if (evento != null)
            {
                await _eRepository.Excluir(evento);
            }
        }

        public async Task InserirEvento(Evento evento)
        {
            await _eRepository.Inserir(evento);
        }

        public async Task<Evento> ObterEvento(int id)
        {
            return await _eRepository.Obter(id);
        }

        public async Task<ICollection<Evento>> ObterTodosEventos()
        {
            return await _eRepository.ObterTodos();
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
            if (ong != null)
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
            return await _oRepository.Obter(id);
        }

        public async Task<ICollection<ONG>> ObterTodasONG()
        {
            return await _oRepository.ObterTodos();
        }

        public async Task<ONG> ObterONGEventos(int id)
        {
            return await _oRepository.ObterONGEventos(id);
        }

        #endregion
    }
}