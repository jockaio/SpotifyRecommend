using SpotifyRecommend.SpotifyService.RequestObjects;
using SpotifyRecommend.SpotifyService.ResponseObjects;
using System.Collections.Generic;

namespace SpotifyRecommend.Models
{
    public class HomeModel
    {
        public string UserName { get; set; }
        public string UserUrl { get; set; }
        public SpotifyGenres SpotifyGenres { get; set; }
        public List<Category> Categories { get; set; }
        public List<Playlist> Playlists { get; set; }
        public TracksSearchResponse TopTracks { get; set; }
        public List<Track> RecommendedTracks { get; set; }
        public RecommendationRequest UserPreferences { get; set; }
    }
}