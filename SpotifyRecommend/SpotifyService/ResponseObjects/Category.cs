using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    public partial class Category
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("icons")]
        public List<Icon> Icons { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}