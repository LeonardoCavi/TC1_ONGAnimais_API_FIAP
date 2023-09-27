﻿using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using ONGAnimaisAPI.Application.ViewModels.Usuario;

namespace ONGAnimaisAPI.Application.Interfaces
{
    public interface IUsuarioApplicationService
    {
        #region [Usuario]
        Task InserirUsuario(InsereUsuarioViewModel usuario);

        Task<ObtemUsuarioViewModel> ObterUsuario(int id);

        Task<ICollection<ObtemUsuarioViewModel>> ObterTodosUsuarios();

        Task<ObtemUsuarioEventosViewModel> ObterUsuarioEventos(int id);

        Task<ObtemUsuarioONGsViewModel> ObterUsuarioONGs(int id);

        Task AtualizarUsuario(AtualizaUsuarioViewModel usuario);

        Task ExcluirUsuario(int id);

        #endregion

        #region [Evento]

        Task SeguirEvento(int eventoId, int id);

        Task DesseguirEvento(int eventoId, int id);

        #endregion

        #region [ONG]

        Task SeguirONG(int ongId, int id);

        Task DesseguirONG(int ongId, int id);

        #endregion
    }
}