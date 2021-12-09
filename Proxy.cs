using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Cross.Proxy.Configs;
using Microsoft.Extensions.Options;

namespace Cross.Proxy
{
    public class Proxy : IProxy
    {
        private readonly HttpClient _client;
        private readonly ProxySettings _options;

        public Proxy(HttpClient client, IOptions<ProxySettings> options)
        {
            _client = client;
            _options = options.Value;
        }


        public void SetDefaultConfig(string clientName)
        {

            if (clientName == null) throw new ArgumentNullException();
            var configs = _options.Clients.First(x => x.Name == clientName);
            if (configs == null) return;

            var bearerToken = configs.TokenAuth;
            var headers = configs.Headers;

            if (!string.IsNullOrEmpty(configs.TokenAuth.Token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Token);
            }

            if (headers.Length > 0)
            {
                AddHeaders(_client, headers);
            }

            if (configs.UriBase != null)
            {
                _client.BaseAddress = configs.UriBase;
            }
        }

        public async Task<TModel> Get<TModel>(Uri uri = null) where TModel : class
        {

            var response = await _client.GetAsync(_client.BaseAddress ?? uri);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }
        public async Task<TModel> Get<TModel>(Uri uri, KeyValuePair<string, string> auth) where TModel : class
        {
            (string key, string value) = auth;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key}:{value}")));
            var response = await _client.GetAsync(uri);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }
        public async Task<TModel> Get<TModel>(Uri uri, KeyValuePair<string, string> auth, params KeyValuePair<string, string>[] headers) where TModel : class
        {
            (string key, string value) = auth;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key}:{value}")));

            AddHeaders(_client, headers);

            var response = await _client.GetAsync(uri);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }
        public async Task<TModel> Get<TModel>(Uri uri, string token) where TModel : class
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync(uri);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }
        public async Task<TModel> PostAsync<TModel>([NotNull] object body, Uri url = null, string endPoint = null, string token = null) where TModel : class
        {
            if (token != null) _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_client.BaseAddress ?? url}{endPoint ?? ""}"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await _client.SendAsync(request);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }
        public async Task PostAsync(Uri url, [NotNull] object body, string endPoint = null, string token = null) 
        {
            if (token != null) _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{url}{endPoint ?? ""}"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        public async Task PutAsync(Uri url, object body, string endPoint = null)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{url}{endPoint ?? ""}"),
                Content = body != null ? new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                ) : null
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        public async Task<TModel> PutAsync<TModel>(Uri url, object body, string endPoint = null, string token = null) where TModel : class
        {
            if (token != null) _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{url}{endPoint ?? ""}"),
                Content = body != null ? new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                ) : null
            };

            var response = await _client.SendAsync(request);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }
        public async Task<TModel> DeleteAsync<TModel>(Uri url, object body = null, string endPoint = null) where TModel : class
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{url}{endPoint ?? ""}"),
                Content = body != null ? new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                ) : null
            };

            var response = await _client.SendAsync(request);
            var content = JsonConvert.DeserializeObject<TModel>(await response.Content.ReadAsStringAsync());
            return content;
        }

        private static void AddHeaders(HttpClient client, params KeyValuePair<string, string>[] headers)
        {
            foreach ((string key, string value) in headers)
            {
                client.DefaultRequestHeaders.Add(key, value);
            }
        }
        
    }
}
