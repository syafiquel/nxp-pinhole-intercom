using System;
using System.Net.Http;

namespace IntercomCameraWebApi.Services
{

    public interface IHttpClientService
    {
        HttpClient CreateClient();
    }

    public class HttpClientService : IHttpClientService
    {
       
        private readonly HttpClient _httpClient;

        public HttpClientService() {

            _httpClient = new HttpClient();

        }

        public HttpClient CreateClient() {
            return _httpClient;
        }
        
    }
}