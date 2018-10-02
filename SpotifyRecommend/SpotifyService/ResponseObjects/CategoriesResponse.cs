namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class CategoriesResponse
    {
        [JsonProperty("categories")]
        public CategoriesData CategoriesData { get; set; }
    }

    public partial class CategoriesData
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("items")]
        public List<Category> Categories { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}