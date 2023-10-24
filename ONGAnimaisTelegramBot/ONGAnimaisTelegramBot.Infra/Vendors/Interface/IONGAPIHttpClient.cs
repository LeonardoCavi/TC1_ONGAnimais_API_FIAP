using ONGAnimaisTelegramBot.Infra.Vendors.Entities;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Interface
{
    public interface IONGAPIHttpClient
    {
        Task GerarToken();

        Task<ONG> ObterOng(int id);

        Task<ICollection<ONG>> ObterOngsCidade(string cidade, string uf, int paginacao = 0);

        Task<ICollection<ONG>> ObterOngsCidadeGeo(string cidade, string uf, int paginacao = 0);

        Task<ONG> ObterONGEventos(int id);

        Task<ICollection<Evento>> ObterEventosCidade(string cidade, string uf, int paginacao = 0);

        Task<ICollection<Evento>> ObterEventosCidadeGeo(string cidade, string uf, int paginacao = 0);

        Task<Evento> ObterEvento(int ongId, int id);

        Task<Usuario> ObterUsuario(int id);

        Task<Usuario> ObterUsuarioPorTelegramId(string telegramId);

        Task<Usuario> ObterUsuarioEventos(int id);

        Task<Usuario> ObterUsuarioONGs(int id);

        Task<bool> SeguirEvento(int usuarioId, int id);

        Task<bool> DesseguirEvento(int usuarioId, int id);

        Task<bool> SeguirONG(int usuarioId, int id);

        Task<bool> DesseguirONG(int usuarioId, int id);

        Task<Usuario> InserirUsuario(Usuario usuario);
    }
}