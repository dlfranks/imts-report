using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Core;

namespace Infrastructure.Core
{
    public class WebService<T> : IWebService<T>
    {
        private readonly HttpClient _client;

        //public T data { get; set; }

        public string apiUrl { get; set; }
        public WebService()
        {
            _client = new HttpClient();
        }
        public WebService(HttpClient client)
        {
            _client = client;
        }
        public async Task<Result<T>> OnGetData(string url, bool isSample = false)
        {
            url = isSample ? url + "&isSample=true" : url;
            var request = new HttpRequestMessage(HttpMethod.Get,
                url);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
            };
            options.Converters.Add(new TimeSpanToStringConverter());

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                object data = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
                return Result<T>.Success((T)data);
            }
            else
            {
                return Result<T>.Failure("HttpClient Failed.");
            }
        }
    }

}