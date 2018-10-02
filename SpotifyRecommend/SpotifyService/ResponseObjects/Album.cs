using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    public partial class Album
    {
        [JsonProperty("album_type")]
        public string AlbumType { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public ReleaseDate ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial struct ReleaseDate
    {
        public DateTimeOffset? DateTime;
        public long? Integer;

        public static implicit operator ReleaseDate(DateTimeOffset DateTime) => new ReleaseDate { DateTime = DateTime };

        public static implicit operator ReleaseDate(long Integer) => new ReleaseDate { Integer = Integer };
    }
}