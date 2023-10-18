using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Domain.Entities
{
    public class Evento : EntidadeBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime Data { get; set; }
        public int OngId { get; set; }
    }
}