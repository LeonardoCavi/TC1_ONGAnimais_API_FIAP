using System.Text.Json.Serialization;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities
{
    public class Autenticacao
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime Expericao { get; set; }
    }
}