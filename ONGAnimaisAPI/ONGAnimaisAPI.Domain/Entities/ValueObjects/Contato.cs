namespace ONGAnimaisAPI.Domain.Entities.ValueObjects
{
    public class Contato: ValueObject
    {
        public string Descricao { get; set; }
        public string URL { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Descricao;
            yield return URL;
        }
    }
}
