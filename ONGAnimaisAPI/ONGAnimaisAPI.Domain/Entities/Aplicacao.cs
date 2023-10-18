namespace ONGAnimaisAPI.Domain.Entities
{
    public class Aplicacao : EntidadeBase
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string NomeAplicacao { get; set; }
    }
}