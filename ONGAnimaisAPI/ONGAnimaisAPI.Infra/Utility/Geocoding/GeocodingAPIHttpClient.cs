using Microsoft.Extensions.Configuration;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using ONGAnimaisAPI.Domain.Interfaces.Utility;
using ONGAnimaisAPI.Infra.Utility.Geocoding.Entities;
using System.Globalization;
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
            Token = _configuration.GetValue<string>("BingMapsAPI:APIKey");
            BaseUri = _configuration.GetValue<string>("BingMapsAPI:BaseUri");
            var end = $"{endereco.Numero} {endereco.Logradouro} " +
                $"{endereco.Bairro}, {endereco.CEP}, {endereco.Cidade}-{endereco.UF}";
            var url = BaseUri + $"?q={end}&key={Token}";

            var result = await _httpHelp.Send(url, null, VerboHttp.Get, null);
            if (result.Code == CodeHttp.Sucess)
            {
                var endResponse = JsonSerializer.Deserialize<BingMapsResponse>(result.Received);
                var localizacao = endResponse.ResourceSets[0].Resources[0].GeocodePoints[0];
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

        public async Task<Endereco> BuscarEnderecoPorGeoLocalizacao(decimal latitude, decimal longitude)
        {
            Token = _configuration.GetValue<string>("BingMapsAPI:APIKey");
            BaseUri = _configuration.GetValue<string>("BingMapsAPI:BaseUri");
            var localidade = $"/{latitude.ToString().Replace(",",".")},{longitude.ToString().Replace(",", ".")}";
            var url = $"{BaseUri}{localidade}?key={Token}";

            var result = await _httpHelp.Send(url, null, VerboHttp.Get, null);
            if (result.Code == CodeHttp.Sucess)
            {
                var endResponse = JsonSerializer.Deserialize<BingMapsResponse>(result.Received);
                var address = endResponse.ResourceSets[0].Resources[0].Address;
                var addressLineSplitted = address.addressLine.Split(',');
                var logradouro = addressLineSplitted[0];
                var numero = addressLineSplitted.Length > 1 ? addressLineSplitted[1].Trim() : "";
                var cep = address.postalCode.Replace("-", "");
                var cidade = address.locality;
                var UF = EstadoParaUF(address.adminDistrict);
                return new()
                {
                    Logradouro = logradouro,
                    Numero = numero,
                    CEP = cep,
                    Cidade = cidade,
                    UF = UF,
                };
            }
            else
            {
                return null;
            }
        }

        private string EstadoParaUF(string estado)
        {
            switch (estado)
            {
                case "Rondônia":
                    return "RO";
                case "Acre":
                    return "AC";
                case "Amazonas":
                    return "AM";
                case "Roraima":
                    return "RR";
                case "Pará":
                    return "PA";
                case "Amapá":
                    return "AP";
                case "Tocantins":
                    return "TO";
                case "Maranhão":
                    return "MA";
                case "Piauí":
                    return "PI";
                case "Ceará":
                    return "CE";
                case "Rio Grande do Norte":
                    return "RN";
                case "Paraíba":
                    return "PB";
                case "Pernambuco":
                    return "PE";
                case "Alagoas":
                    return "AL";
                case "Sergipe":
                    return "SE";
                case "Bahia":
                    return "BA";
                case "Minas Gerais":
                    return "MG";
                case "Espírito Santo":
                    return "ES";
                case "Rio de Janeiro":
                    return "RJ";
                case "São Paulo":
                    return "SP";
                case "Paraná":
                    return "PR";
                case "Santa Catarina":
                    return "SC";
                case "Rio Grande do Sul":
                    return "RS";
                case "Mato Grosso do Sul":
                    return "MS";
                case "Mato Grosso":
                    return "MT";
                case "Goiás":
                    return "GO";
                case "Distrito Federal":
                    return "DF";
                default:
                    return "";
            }
        }
    }
}