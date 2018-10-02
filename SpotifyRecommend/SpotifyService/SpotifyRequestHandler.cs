using SpotifyRecommend.SpotifyService.Helpers;
using SpotifyRecommend.SpotifyService.ResponseObjects;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SpotifyRecommend.SpotifyService
{
    public class SpotifyRequestHandler
    {
        private HttpClient _client;
        private string _baseAddress = "https://api.spotify.com/v1/"; //Move to config

        public SpotifyRequestHandler(string accessToken)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        #region helper methods to make requests

        private async Task<HttpResponseMessage> SendGetRequest(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _baseAddress + endpoint);
            return await _client.SendAsync(request);
        }

        private async Task<T> GetSpotifyResponseObject<T>(string endpoint)
        {
            var response = await SendGetRequest(endpoint);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return SpotifyResponseObjectConverter.FromJson<T>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                Debug.WriteLine("====== Unsuccessful Request =====");
                Debug.WriteLine(response.StatusCode);
                Debug.WriteLine(response.ReasonPhrase);
                Debug.WriteLine(response.RequestMessage);

                return default(T);
            }
        }

        #endregion helper methods to make requests

        public async Task<SpotifyGenres> GetGenresAsync()
        {
            return await GetSpotifyResponseObject<SpotifyGenres>("recommendations/available-genre-seeds");
        }

        public async Task<CategoriesResponse> GetCategoriesAsync()
        {
            return await GetSpotifyResponseObject<CategoriesResponse>("browse/categories");
        }

        public async Task<TracksSearchResponse> GetUserTopTracksAsync()
        {
            return await GetSpotifyResponseObject<TracksSearchResponse>("me/top/tracks");
        }

        public async Task<PlaylistResponse> GetCategoryPlaylistsAsync(string categoryID)
        {
            return await GetSpotifyResponseObject<PlaylistResponse>("browse/categories/" + categoryID + "/playlists");
        }

        public async Task<TracksSearchResponse> GetPlaylistTracksAsync(string playlistID)
        {
            PlaylistTracks pt = await GetSpotifyResponseObject<PlaylistTracks>("playlists/" + playlistID + "/tracks?offset=" + OffsetSelector.GetRandomOffset());
            TracksSearchResponse tsr = new TracksSearchResponse
            {
                Tracks = pt.Tracks.Select(x => x.Track).ToList()
            };

            return tsr;
        }

        public async Task<RecommendationsResponse> GetTrackRecommendations(string query)
        {
            return await GetSpotifyResponseObject<RecommendationsResponse>("recommendations" + query);
        }
    }
}