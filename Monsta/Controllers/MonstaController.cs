using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monsta.dto.App;
using Monsta.dto.genres;
using Monsta.Services;
using Newtonsoft.Json;

namespace Monsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonstaController : ControllerBase
    {

        public GenreService _genreService { get; set; }

        public AppService _appService { get; set; }

        public MonstaController(GenreService genreService,AppService appService)
        {
            _genreService = genreService;
            _appService = appService;
        }

        [HttpGet("genresRanking")]
        public async Task<IActionResult> getGenres(string store,string date)
        {

            HttpResponseMessage response = await _genreService.getGenereRankings(store,date);

            Stream responseStream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                // Create a list to store the converted objects
                List<GenreRankingDto> genreRankings = new List<GenreRankingDto>();

                // Create a StreamReader to read the response stream
                using (StreamReader streamReader = new StreamReader(responseStream))
                {
                    // Read the response stream line by line
                    string line;
                    while ((line = await streamReader.ReadLineAsync()) != null)
                    {
                        // Deserialize the individual object to your custom type
                        GenreRankingDto genreRanking = JsonConvert.DeserializeObject<GenreRankingDto>(line);
                        genreRankings.Add(genreRanking);
                    }
                }

                Console.WriteLine(genreRankings.Count);

                return Ok(genreRankings);
            }
            else
            {
                var data = response.Content.ReadAsStringAsync().Result;
                // Handle the failure scenario
                return StatusCode((int)response.StatusCode, data);
            }
        }



        [HttpGet("aggregatedGenres")]
        public async Task<IActionResult> aggregatedGenres(string country,string date,string store)
        {
            Console.WriteLine("country" + country);
            List<Genre> genres = await _genreService.getAggregatedGenres(country, date, store);

            if(genres == null)
            {
                return StatusCode(400,new {errorMsg = "UnAuthorized to this content"});
            }
            return Ok(genres);
        }




        [HttpGet("AppDetails/{app_id}")]
        public async Task<IActionResult> getAppDetails(string app_id,string country,string store)
        {

            App app = await _appService.getApp(app_id, country, store);

            if(app == null)
            {
                return StatusCode(400, new { errorMsg = "Somthing went wrong" });
            }
            return Ok(app);
        }
    }
}
