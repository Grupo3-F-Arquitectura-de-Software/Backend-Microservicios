using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace VehicleService.Infrastructure.Http
{
    public class AuthenticatedHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedHttpClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .FirstOrDefault()?.Replace("Bearer ", "");
            
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = content
            };
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return await SendAsync(request);
        }
    }
} 