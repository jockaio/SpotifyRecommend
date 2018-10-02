using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SpotifyRecommend.Models;
using SpotifyRecommend.SpotifyService.RequestObjects;
using SpotifyRecommend.SpotifyService.ResponseObjects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpotifyRecommend.Controllers
{
    public class HomeController : Controller
    {
        private SpotifyService.SpotifyService _spotifyService;

        private async Task<SpotifyService.SpotifyService> GetSpotifyServiceAsync()
        {
            return new SpotifyService.SpotifyService(await HttpContext.GetTokenAsync("access_token"));
        }

        public async Task<IActionResult> Index()
        {
            if (_spotifyService == null)
            {
                _spotifyService = await GetSpotifyServiceAsync();
            }

            HomeModel model = new HomeModel
            {
                UserPreferences = new RecommendationRequest() //Set defaults for recommendation request
                {
                    MinDanceAbility = 50,
                    TargetTempo = 60
                }
            };

            if (User.Identity.IsAuthenticated)
            {
                model.UserName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    model.UserName = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                model.UserUrl = User.FindFirst(c => c.Type == "urn:spotify_url")?.Value;

                model.SpotifyGenres = await _spotifyService.GetGenres();
                var catRes = await _spotifyService.GetCategories();
                model.Categories = catRes.CategoriesData.Categories;
                model.TopTracks = await _spotifyService.GetUserTopTracks();
            }
            return View(model);
        }

        public async Task<IActionResult> Recommendations(RecommendationRequest request)
        {
            if (_spotifyService == null)
            {
                _spotifyService = await GetSpotifyServiceAsync();
            }

            var recommendations = await _spotifyService.GetRecommendationsAsync(request);
            if (recommendations != null)
            {
                var recommendedTracks = recommendations.Tracks;

                return PartialView(recommendedTracks);
            }
            else
            {
                return BadRequest("Not found.");
            }
        }

        public async Task<IActionResult> CategoryPlaylists(RecommendationRequest request)
        {
            if (_spotifyService == null)
            {
                _spotifyService = await GetSpotifyServiceAsync();
            }

            List<Playlist> playlists = new List<Playlist>();

            if (request.SeedCategories == null)
            {
                return PartialView(playlists);
            }

            foreach (var category in request.SeedCategories)
            {
                var pr = await _spotifyService.GetCategoryPlaylists(category);
                playlists.AddRange(pr.PlaylistsData.Playlists);
            }
            if (playlists.Any())
            {
                return PartialView(playlists);
            }
            else
            {
                return BadRequest("Not found.");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }
}