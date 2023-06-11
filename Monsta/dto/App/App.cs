namespace Monsta.dto.App
{
    public class App
    {
        public string id { get; set; } = string.Empty;

        public string app_name { get; set; } = string.Empty;

        public string publisher_name { get; set; } = string.Empty;

        public double all_rating { get; set; } = 0;

        public string icon_url { get; set; } = string.Empty;

        public string genre { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public List<string> screenshot_urls { get; set; } = new List<string>();
    }
}
