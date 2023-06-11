using Monsta.dto.genres;

namespace Monsta.Services
{
    public class ListsService
    {

        public List<GenreRankingDto> genreRankingDtos { get; set; } = new List<GenreRankingDto>();

        public List<Genre> genres { get; set; }
    }
}
