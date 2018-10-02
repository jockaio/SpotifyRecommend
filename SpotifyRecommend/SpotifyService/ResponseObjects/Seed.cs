using Newtonsoft.Json;
using System;

namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    public partial class Seed
    {
        [JsonProperty("initialPoolSize")]
        public long InitialPoolSize { get; set; }

        [JsonProperty("afterFilteringSize")]
        public long AfterFilteringSize { get; set; }

        [JsonProperty("afterRelinkingSize")]
        public long AfterRelinkingSize { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}