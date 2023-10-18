using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities.Enums
{
    public enum TipoTelefone
    {
        [JsonPropertyName("Fixo")]
        Fixo,
        [JsonPropertyName("Celular")]
        Celular
    }
}