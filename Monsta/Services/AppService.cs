using Monsta.dto.App;
using System.Net.Http.Headers;
using System;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Caching.Memory;

namespace Monsta.Services
{
    public class AppService
    {
        public IConfiguration _config { get; set; }
        public HttpClient _httpClient { get; set; }

        private readonly IMemoryCache _cache;

        public AppService(IConfiguration config, HttpClient httpClient,IMemoryCache cache)
        {
            _config = config;
            _httpClient = httpClient;
            _cache = cache;
        }


        public async Task<App> getApp(string app_id,string country,string store)
        {
            string cacheKey = app_id + country + store;

            if (_cache.TryGetValue(cacheKey,out App cachedApp))
            {
                Console.WriteLine("handle app request in local api");
                return cachedApp;
            }


            Console.WriteLine("handle app request in monsta api");

            string base_url = _config.GetSection("URLS:MonstaBaseUrl").Value;

            var url = $"{base_url}{store}/details/{app_id}.json?country={country}";

            string username = _config.GetSection("URLS:KEY").Value;

            string password = "X";

            // Set authentication header
            string authValue = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            // Send the HTTP request
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                App app = await response.Content.ReadFromJsonAsync<App>();
                _cache.Set(cacheKey,app);
                return app;
            }

            return null;
        }
    }
}
