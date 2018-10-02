namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class TracksSearchResponse
    {
        [JsonProperty("items")]
        public List<Track> Tracks { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("previous")]
        public Uri Previous { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }
    }
}