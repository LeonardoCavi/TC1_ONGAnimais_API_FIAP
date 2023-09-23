using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Application.ViewModels.Evento
{
    public class AtualizaEventoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime Data { get; set; }
    }
}
