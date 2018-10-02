using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    public partial class RecommendationsResponse
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }

        [JsonProperty("seeds")]
        public List<Seed> Seeds { get; set; }
    }
}