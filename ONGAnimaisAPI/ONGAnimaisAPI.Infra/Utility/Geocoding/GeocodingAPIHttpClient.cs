using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using ONGAnimaisAPI.Domain.Interfaces.Utility;
using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ONGAnimaisAPI.Infra.Utility.Geocoding
{
    public class GeocodingAPIHttpClient : IGeocodingAPIHttpClient
    {
        private static string Token;
        private static string BaseUri;
        private readonly string ClassName = typeof(GeocodingAPIHttpClient).Name;
        private readonly HttpHelp _httpHelp;
        private readonly IConfiguration _configuration;

        public GeocodingAPIHttpClient(HttpHelp httpHelp,
                                      IConfiguration configuration)
        {
            _httpHelp = httpHelp;
            _configuration = configuration;
        }

        public async Task<GeoLocalizacao> BuscarLatLongPorEndereco(Endereco endereco)
        {
            //Token = _configuration.GetValue<string>("APIGeocoding:APIKey");
            //BaseUri = _configuration.GetValue<string>("APIGeocoding:BaseUri");
            Token = _configuration.GetValue<string>("BingMapsAPI:APIKey");
            BaseUri = _configuration.GetValue<string>("BingMapsAPI:BaseUri");
            var end = $"{endereco.Numero} {endereco.Logradouro} " +
                $"{endereco.Bairro}, {endereco.CEP}, {endereco.Cidade}-{endereco.UF}";
            var url = BaseUri + $"?q={end}&key={Token}";

            var result = await _httpHelp.Send(url, null, VerboHttp.Get, null);
            if (result.Code == CodeHttp.Sucess)
            {
                //var endResponse = JsonSerializer.Deserialize<OpenCageResponse>(result.Received);
                var endResponse = JsonSerializer.Deserialize<BingMapsResponse>(result.Received);
                //var localizacao = endResponse.results[0].geometry;
                var localizacao = endResponse.ResourceSets[0].Resources[0].GeocodePoints[0];
                //decimal latitude = localizacao.lat;
                //decimal longitude = localizacao.lng;
                decimal latitude = localizacao.Coordinates[0];
                decimal longitude = localizacao.Coordinates[1];

                GeoLocalizacao geoRetorno = new GeoLocalizacao();
                geoRetorno.Latitude = latitude;
                geoRetorno.Longitude = longitude;

                return geoRetorno;
            }
            else
            {
                return null;
            }
        }
    }

    public class BingMapsResponse
    {
        [JsonPropertyName("resourceSets")]
        public ResourceSets[] ResourceSets { get; set; }
    }

    public class ResourceSets
    {
        [JsonPropertyName("resources")]
        public Resources[] Resources { get; set; }
    }

    public class Resources
    {
        [JsonPropertyName("geocodePoints")]
        public GeocodePoints[] GeocodePoints { get; set; }
    }

    public class GeocodePoints
    {
        [JsonPropertyName("coordinates")]
        public decimal[] Coordinates { get; set; }
    }

    public class OpenCageResponse
    {
        public OpenCageResult[] results { get; set; }
    }

    public class OpenCageResult
    {
        public Geometry geometry { get; set; }
    }

    public class Geometry
    {
        public decimal lat { get; set; }
        public decimal lng { get; set; }
    }
}