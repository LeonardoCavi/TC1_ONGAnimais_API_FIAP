using Microsoft.Extensions.Logging;
using Polly;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ONGAnimaisTelegramBot.Infra.Utility
{
    public enum VerboHttp
    {
        Get,
        Post,
        Put,
        Delete
    }

    public enum CodeHttp
    {
        Sucess,
        BadRequest,
        ServerError,
        Others
    }

    public class Response
    {
        public CodeHttp Code { get; set; }
        public string Received { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class HttpHelp
    {
        private readonly ILogger<HttpHelp> _logger;
        private readonly string className = typeof(HttpHelp).Name;
        private readonly AsyncPolicy _resiliencePolicy;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpHelp(ILogger<HttpHelp> logger,
                        AsyncPolicy resiliencePolicy,
                        IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _resiliencePolicy = resiliencePolicy;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Response> Send(string url, string jsonRequest, VerboHttp verboHttp, Dictionary<string, string> headers = null)
        {
            try
            {
                using var result = await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    return await ExternalIntegration(url, jsonRequest, verboHttp, headers);
                });

                var response = await ProcessAndAnalyzeResponse(result, url);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{className} - url:{url} Send Ex:{ex}");

                Response rtd = new Response
                {
                    Received = null,
                    Code = CodeHttp.ServerError
                };

                return rtd;
            }
        }

        private async Task<HttpResponseMessage> ExternalIntegration(string url, string jsonRequest, VerboHttp verboHttp, Dictionary<string, string> headers = null)
        {
            using var httpClient = _httpClientFactory.CreateClient("HTTPOngAPI");
            HttpResponseMessage result = null;
            if(headers != null)
            {
                foreach(var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            switch (verboHttp)
            {
                case VerboHttp.Get:
                    result = await httpClient.GetAsync(url);
                    break;

                case VerboHttp.Post:
                    StringContent contentPost = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    result = await httpClient.PostAsync(url, contentPost);
                    contentPost.Dispose();
                    break;

                case VerboHttp.Put:
                    if (string.IsNullOrEmpty(jsonRequest))
                    {
                        result = await httpClient.PutAsync(url, null);
                    }
                    else
                    {
                        StringContent contentPut = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                        result = await httpClient.PutAsync(url, contentPut);
                        contentPut.Dispose();
                    }
                    break;

                case VerboHttp.Delete:
                    //StringContent contentDelete = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    result = await httpClient.DeleteAsync(url);
                    //contentDelete.Dispose();
                    break;
            }

            return result;
        }

        private async Task<Response> ProcessAndAnalyzeResponse(HttpResponseMessage result, string uri)
        {
            Response response = new Response();
            string content = string.Empty;

            try
            {
                content = await result.Content.ReadAsStringAsync();

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        response.Code = CodeHttp.Sucess;
                        response.Received = content;
                        return response;

                    case HttpStatusCode.Created:
                        response.Code = CodeHttp.Sucess;
                        response.Received = content;
                        return response;

                    case HttpStatusCode.Accepted:
                        response.Code = CodeHttp.Sucess;
                        response.Received = content;
                        return response;

                    case HttpStatusCode.NoContent:
                        response.Code = CodeHttp.Sucess;
                        response.Received = content;
                        return response;

                    case HttpStatusCode.ResetContent:
                        response.Code = CodeHttp.Sucess;
                        response.Received = content;
                        return response;

                    case HttpStatusCode.BadRequest:
                        response.Code = CodeHttp.BadRequest;
                        response.Received = content;
                        return response;

                    case HttpStatusCode.InternalServerError:
                        response.Code = CodeHttp.ServerError;
                        response.Received = content;
                        return response;

                    default:
                        response.Code = CodeHttp.Others;
                        response.Received = content;
                        return response;
                }
            }
            catch (Exception ex)
            {
                response.Code = CodeHttp.ServerError;
                response.Received = ex.ToString();
                return response;
            }
            finally
            {
                _logger.LogInformation($"{className}: ProcessAndAnalyzeResponse{Environment.NewLine}URL:{uri}{Environment.NewLine}Http Status Code:{result.StatusCode}{Environment.NewLine}Content:{content}");
                result.Dispose();
            }
        }
    }
}