using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Domain.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; set; }
        public Telefone Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public List<Evento> EventosSeguidos { get; set; }
        public List<ONG> ONGsSeguidas { get; set; }
    }
}