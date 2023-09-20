using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Repository;
using ONGAnimaisAPI.Domain.Interfaces.Services;
using System.Security.Cryptography;

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

        public async Task AtualizarONG(ONG ong)
        {
            var ongDB = await _oRepository.Obter(ong.Id);
            if (ongDB != null)
            {
                await _oRepository.Atualizar(ong);
            }
        }

        public async Task ExcluirONG(int id)
        {
            var ong = await _oRepository.Obter(id);
            if(ong != null)
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
    }
}