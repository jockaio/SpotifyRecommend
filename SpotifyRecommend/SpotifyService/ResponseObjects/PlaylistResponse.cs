namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class PlaylistResponse
    {
        [JsonProperty("playlists")]
        public PlaylistsData PlaylistsData { get; set; }
    }

    public partial class PlaylistsData
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("items")]
        public List<Playlist> Playlists { get; set; }

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

    public partial class Playlist
    {
        [JsonProperty("collaborative")]
        public bool Collaborative { get; set; }

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

        [JsonProperty("primary_color")]
        public object PrimaryColor { get; set; }

        [JsonProperty("public")]
        public object Public { get; set; }

        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class PlaylistTracks
    {
        [JsonProperty("items")]
        public List<PlaylistTrack> Tracks { get; set; }
    }

    public partial class PlaylistTrack
    {
        [JsonProperty("track")]
        public Track Track { get; set; }
    }
}