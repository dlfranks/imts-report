using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Models.Core;
using API.Models.FieldConcreteTest;

namespace API.Services
{
    public class WebService<T> : IWebService<T>
    {
        private readonly HttpClient _client;
        
        public T data { get; set; }

        public string apiUrl { get; set; }

        public WebService(HttpClient client)
        {
            _client = client;

        }

        public async Task<Result<T>> OnGetData(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                url);
            request.Headers.Add("Accept", "application/json");

            
            

            var response = await _client.SendAsync(request);
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // options.Converters.Add(new CustomJsonConverterForNullableDateTime(options));
            options.Converters.Add(new TimeSpanToStringConverter());

            

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                data = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
            }
            
            return Result<T>.Success(data);
        }


    }

    public class CustomJsonConverterForNullableDateTime : JsonConverter<DateTime?>
    {
        protected JsonSerializerOptions options;
        public CustomJsonConverterForNullableDateTime(JsonSerializerOptions options)
        {
            this.options = options;
        }
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Console.WriteLine("Reading");
            Debug.Assert(typeToConvert == typeof(DateTime?));
            return reader.GetString() == "" ? null : reader.GetDateTime();
        }

        // This method will be ignored on serialization, and the default typeof(DateTime) converter is used instead.
        // This is a bug: https://github.com/dotnet/corefx/issues/41070#issuecomment-560949493
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            Console.WriteLine("Here - writing");

            if (!value.HasValue)
            {
                writer.WriteStringValue("");
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        }
    }

    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value=reader.GetString();
            return TimeSpan.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}