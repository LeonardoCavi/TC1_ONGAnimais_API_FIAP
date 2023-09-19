using ONGAnimaisAPI.Domain.Enums;

namespace ONGAnimaisAPI.Domain.Entities.ValueObjects
{
    public class Telefone
    {
        public int DDD { get; set; }
        public int Numero { get; set; }
        public TipoTelefone Tipo { get; set; }
    }
}