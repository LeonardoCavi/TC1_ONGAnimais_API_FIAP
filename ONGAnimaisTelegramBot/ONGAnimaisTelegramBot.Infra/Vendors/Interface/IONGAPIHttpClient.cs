using ONGAnimaisTelegramBot.Infra.Vendors.Entities;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Interface
{
    public interface IONGAPIHttpClient
    {
        Task GerarToken();

        Task<ONG> ObterOng(int id);

        Task<List<ONG>> ObterOngsCidade(string cidade, string uf, int paginacao = 0);

        Task<List<ONG>> ObterOngsGeo(decimal latitude, decimal longitude, int paginacao = 0);

        Task<List<ONG>> ObterOngsGeo(int id, decimal latitude, decimal longitude, int paginacao = 0);

        Task<ONG> ObterONGEventos(int id);

        Task<List<Evento>> ObterEventosCidade(string cidade, string uf, int paginacao = 0);

        Task<List<Evento>> ObterEventosGeo(decimal latitude, decimal longitude, int paginacao = 0);

        Task<List<Evento>> ObterEventosGeo(int id, decimal latitude, decimal longitude, int paginacao = 0);

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