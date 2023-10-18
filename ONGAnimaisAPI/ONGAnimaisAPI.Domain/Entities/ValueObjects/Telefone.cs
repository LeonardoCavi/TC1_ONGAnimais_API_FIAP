using ONGAnimaisAPI.Domain.Enums;

namespace ONGAnimaisAPI.Domain.Entities.ValueObjects
{
    public class Telefone
    {
        public string DDD { get; set; }
        public string Numero { get ; set; }
        public TipoTelefone Tipo { get; set; }
    }
}