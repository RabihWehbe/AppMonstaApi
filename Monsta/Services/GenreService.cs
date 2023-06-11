using Monsta.dto.genres;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Monsta.Services
{
    public class GenreService
    {
        public IConfiguration _config { get; set; }
        public HttpClient _httpClient;


        public GenreService(IConfiguration config,HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<dynamic> getGenereRankings(string store,string date)
        {
            string base_url = _config.GetSection("URLS:MonstaBaseUrl").Value;


            var url = $"{base_url}{store}/rankings/genres.json?date={date}";

            Console.WriteLine(url);

            //Auth params
            string username = _config.GetSection("URLS:KEY").Value;

            string password = "X";

            // Set authentication header
            string authValue = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            // Send the HTTP request
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            return response;
        }


        


        public async Task<List<Genre>> getAggregatedGenres(string country,string date,string store)
        {
            string base_url = _config.GetSection("URLS:MonstaBaseUrl").Value;

            var url = $"{base_url}{store}/rankings/aggregate.json?country={country}&date={date}";

            Console.WriteLine(url);

            //Auth params
            string username = _config.GetSection("URLS:KEY").Value;

            Console.WriteLine(username);
            string password = "X";

            // Set authentication header
            string authValue = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            var response = await _httpClient.GetAsync(url);

            Stream responseStream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                // Create a list to store the converted objects
                List<Genre> genres = new List<Genre>();

                try
                {
                    // Create a StreamReader to read the response stream
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        // Read the response stream line by line
                        string line;
                        while ((line = await streamReader.ReadLineAsync()) != null)
                        {
                            // Deserialize the individual object to your custom type
                            Genre genre = JsonConvert.DeserializeObject<Genre>(line);
                            genres.Add(genre);
                        }
                    }
                }
                catch (Exception e)
                {
                    return genres;
                }

                return genres;
            }
            else
            {
                return null;
            }
        }
    }
}
