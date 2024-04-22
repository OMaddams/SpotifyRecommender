using Newtonsoft.Json;

namespace SpotifyRecommender.App.Models
{
    internal class AcessTokenModel
    {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime CreationTime { get; private set; } = DateTime.Now;
    }

    public class UserTopTracksModel
    {
        [JsonProperty("href")]
        public string Href;

        [JsonProperty("limit")]
        public int? Limit;

        [JsonProperty("next")]
        public string Next;

        [JsonProperty("offset")]
        public int? Offset;

        [JsonProperty("previous")]
        public string Previous;

        [JsonProperty("total")]
        public int? Total;

        [JsonProperty("items")]
        public List<Item> Items;
    }

    public class Item
    {
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls;

        [JsonProperty("followers")]
        public Followers Followers;

        [JsonProperty("genres")]
        public List<string> Genres;

        [JsonProperty("href")]
        public string Href;

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("images")]
        public List<Image> Images;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("popularity")]
        public int? Popularity;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("uri")]
        public string Uri;
    }
    public class Image
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("height")]
        public int? Height;

        [JsonProperty("width")]
        public int? Width;
    }
    public class ExternalUrls
    {
        [JsonProperty("spotify")]
        public string Spotify;
    }

    public class Followers
    {
        [JsonProperty("href")]
        public string Href;

        [JsonProperty("total")]
        public int? Total;
    }
}
