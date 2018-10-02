using System.Collections.Generic;

namespace SpotifyRecommend.SpotifyService.RequestObjects
{
    public class RecommendationRequest
    {
        public List<string> SeedArtists { get; set; }
        public List<string> SeedGenres { get; set; }
        public List<string> SeedTracks { get; set; }
        public List<string> SeedCategories { get; set; }
        public List<string> SeedPlaylists { get; set; }
        public float Acusticness { get; set; }
        public float MinDanceAbility { get; set; }
        public float MaxDanceAbility { get; set; }
        public float TargetDanceAbility { get; set; }
        public int Duration { get; set; }
        public float Energy { get; set; }
        public float Instrumentalness { get; set; }
        public int Key { get; set; }
        public float Liveness { get; set; }
        public float Loudness { get; set; }
        public int Mode { get; set; }
        public int Popularity { get; set; }
        public float Speechiness { get; set; }
        public float MinTempo { get; set; }
        public float MaxTempo { get; set; }
        public float TargetTempo { get; set; }
        public int TimeSignature { get; set; }
        public float Valence { get; set; }
    }
}