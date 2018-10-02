namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class SpotifyGenres
    {
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
    }
}