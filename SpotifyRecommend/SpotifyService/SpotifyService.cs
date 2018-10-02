using SpotifyRecommend.SpotifyService.RequestObjects;
using SpotifyRecommend.SpotifyService.ResponseObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyRecommend.SpotifyService
{
    public class SpotifyService
    {
        private readonly SpotifyRequestHandler _requestHandler;

        public SpotifyService(string accessToken)
        {
            _requestHandler = new SpotifyRequestHandler(accessToken);
        }

        public async Task<RecommendationsResponse> GetRecommendationsAsync(RecommendationRequest recommendationRequest)
        {
            #region Create seed lists if null

            if (recommendationRequest.SeedArtists == null)
            {
                recommendationRequest.SeedArtists = new List<string>();
            }

            if (recommendationRequest.SeedGenres == null)
            {
                recommendationRequest.SeedGenres = new List<string>();
            }

            if (recommendationRequest.SeedTracks == null)
            {
                recommendationRequest.SeedTracks = new List<string>();
            }

            #endregion Create seed lists if null

            await FillSeedTracks(recommendationRequest);

            string query = FormatRecommendationQuery(recommendationRequest);
            return await _requestHandler.GetTrackRecommendations(query);
        }

        private int GetSeedCount(RecommendationRequest recommendationRequest)
        {
            int result = 0;
            result += recommendationRequest.SeedArtists != null ? recommendationRequest.SeedArtists.Count() : 0;
            result += recommendationRequest.SeedGenres != null ? recommendationRequest.SeedGenres.Count() : 0;
            result += recommendationRequest.SeedTracks != null ? recommendationRequest.SeedTracks.Count() : 0;
            return result;
        }

        private async Task FillSeedTracks(RecommendationRequest recommendationRequest)
        {
            List<TracksSearchResponse> tracksToAdd = new List<TracksSearchResponse>();
            if (recommendationRequest.SeedPlaylists != null && recommendationRequest.SeedPlaylists.Any()) //If the user has selected categories, choose tracks according to those
            {
                foreach (var playlist in recommendationRequest.SeedPlaylists)
                {
                    tracksToAdd.Add(await _requestHandler.GetPlaylistTracksAsync(playlist));
                }
            }
            else // else take user top tracks
            {
                tracksToAdd.Add(await _requestHandler.GetUserTopTracksAsync());
            }

            int tracksFromEach = (5 - GetSeedCount(recommendationRequest)) / tracksToAdd.Count();
            if (tracksFromEach < 1)
            {
                tracksFromEach = 1;
            }
            foreach (var tracksSearch in tracksToAdd)
            {
                foreach (var track in tracksSearch.Tracks.Take(tracksFromEach))
                {
                    if (GetSeedCount(recommendationRequest) < 5)
                    {
                        recommendationRequest.SeedTracks.Add(track.Id);
                    }
                }
            }
        }

        private string FormatRecommendationQuery(RecommendationRequest recommendationRequest)
        {
            StringBuilder stringBuilder = new StringBuilder("?");
            var c = System.Globalization.CultureInfo.InvariantCulture;

            //Add all parameters for the reccomendation request

            #region Build query

            if (recommendationRequest.SeedArtists.Any())
            {
                stringBuilder.Append("seed_artists=");
                foreach (var artist in recommendationRequest.SeedArtists)
                {
                    stringBuilder.Append(artist);
                    if (artist != recommendationRequest.SeedArtists.Last())
                    {
                        stringBuilder.Append(",");
                    }
                }
            }

            if (recommendationRequest.SeedGenres.Any())
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Append("&");
                }
                stringBuilder.Append("seed_genres=");
                foreach (var genre in recommendationRequest.SeedGenres)
                {
                    stringBuilder.Append(genre);
                    if (genre != recommendationRequest.SeedGenres.Last())
                    {
                        stringBuilder.Append(",");
                    }
                }
            }

            if (recommendationRequest.SeedTracks.Any())
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Append("&");
                }
                stringBuilder.Append("seed_tracks=");
                foreach (var track in recommendationRequest.SeedTracks)
                {
                    stringBuilder.Append(track);
                    if (track != recommendationRequest.SeedTracks.Last())
                    {
                        stringBuilder.Append(",");
                    }
                }
            }

            if (recommendationRequest.MinDanceAbility > 0)
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Append("&");
                }

                stringBuilder.Append("min_danceability=");
                stringBuilder.Append((recommendationRequest.MinDanceAbility / 100).ToString("0.00", c));
            }

            if (recommendationRequest.MaxDanceAbility > 0)
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Append("&");
                }

                stringBuilder.Append("max_danceability=");
                stringBuilder.Append((recommendationRequest.MaxDanceAbility / 100).ToString("0.00", c));
            }

            if (recommendationRequest.TargetDanceAbility > 0)
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Append("&");
                }

                stringBuilder.Append("target_danceability=");
                stringBuilder.Append((recommendationRequest.TargetDanceAbility / 100).ToString("0.00", c));
            }

            if (recommendationRequest.TargetTempo > 60)
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Append("&");
                }

                stringBuilder.Append("target_tempo=");
                stringBuilder.Append((recommendationRequest.TargetTempo).ToString("0.00", c));
            }

            #endregion Build query

            return stringBuilder.ToString();
        }

        public Task<TracksSearchResponse> GetUserTopTracks()
        {
            return _requestHandler.GetUserTopTracksAsync();
        }

        public Task<SpotifyGenres> GetGenres()
        {
            return _requestHandler.GetGenresAsync();
        }

        public Task<CategoriesResponse> GetCategories()
        {
            return _requestHandler.GetCategoriesAsync();
        }

        public async Task<PlaylistResponse> GetCategoryPlaylists(string categoryID)
        {
            return await _requestHandler.GetCategoryPlaylistsAsync(categoryID);
        }
    }
}