using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Application.ViewModels.ONG
{
    public class ObtemONGViewModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Responsavel { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; }
        public List<Contato> Contatos { get; set; }
        public List<Evento> Eventos { get; set; }
    }
}
