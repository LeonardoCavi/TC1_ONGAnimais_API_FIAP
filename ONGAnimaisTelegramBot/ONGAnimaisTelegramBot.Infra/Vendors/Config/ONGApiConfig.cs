namespace ONGAnimaisTelegramBot.Infra.Vendors.Config
{
    public class ONGApiConfig
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public OngAPIEndpoint Endpoints { get; set; }
    }

    public class OngAPIEndpoint
    {
        public string BaseUri { get; set; }
        public string TokenEndpoint { get; set; }
        public string ObterONGEndpoint { get; set; }
        public string ObterONGsPorCidadeEndpoint { get; set; }
        public string ObterONGsPorCidadeGeoEndpoint { get; set; }
        public string ObterONGEventosEndpoint { get; set; }
        public string ObterEventoEndpoint { get; set; }
        public string ObterEventosPorCidadeEndpoint { get; set; }
        public string ObterUsuarioEndpoint { get; set; }
        public string ObterUsuarioPorTelegramIdEndpoint { get; set; }
        public string ObterUsuarioEventosEndpoint { get; set; }
        public string ObterEventosPorCidadeGeoEndpoint { get; set; }
        public string ObterUsuarioONGsEndpoint { get; set; }
        public string InserirUsuarioEndpoint { get; set; }
        public string AtualizarUsuarioEndpoint { get; set; }
        public string ExcluirUsuarioEndpoint { get; set; }
        public string SeguirEventoEndpoint { get; set; }
        public string DesseguirEventoEndpoint { get; set; }
        public string SeguirONGEndpoint { get; set; }
        public string DesseguirONGEndpoint { get; set; }
    }
}