using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Application.ViewModels.Usuario
{
    public class ObtemUsuarioEventosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Telefone Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public List<Domain.Entities.Evento> EventosSeguidos { get; set; }
    }
}