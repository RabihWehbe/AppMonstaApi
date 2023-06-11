namespace Monsta.dto.genres
{
    public class Genre
    {

        public List<string> ranks { get; set; } = new List<string>();//list of apps_id

        public string country { get; set; } = string.Empty;

        public string rank_id { get; set; } = string.Empty;

        public string genre_id { get; set; } = string.Empty;
    }
}
