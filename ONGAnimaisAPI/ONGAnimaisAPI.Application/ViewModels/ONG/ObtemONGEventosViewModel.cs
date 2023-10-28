using ONGAnimaisAPI.Domain.Entities.ValueObjects;


namespace ONGAnimaisAPI.Application.ViewModels.ONG
{
    public class ObtemONGEventosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Responsavel { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; }
        public GeoLocalizacao GeoLocalizacao { get; set; }
        public List<Telefone> Telefones { get; set; }
        public List<Contato> Contatos { get; set; }
        public List<Domain.Entities.Evento> Eventos { get; set; }
    }
}