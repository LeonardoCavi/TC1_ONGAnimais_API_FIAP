using ONGAnimaisAPI.Domain.Enums;

namespace ONGAnimaisAPI.Domain.Entities.ValueObjects
{
    public class Telefone : ValueObject
    {
        public string DDD { get; set; }
        public string Numero { get ; set; }
        public TipoTelefone Tipo { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DDD;
            yield return Numero;
            yield return Tipo;
        }
    }
}