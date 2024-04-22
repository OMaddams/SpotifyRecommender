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
}
