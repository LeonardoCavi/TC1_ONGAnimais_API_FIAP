using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Domain.Entities
{
    public class ONG : EntidadeBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Responsavel { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; }
        public List<Contato> Contatos { get; set; }
        
        //public List<Evento> Eventos { get; set; }
    }
}