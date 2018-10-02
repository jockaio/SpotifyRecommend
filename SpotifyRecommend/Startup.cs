using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace SpotifyRecommend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Spotify";
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    // this event is fired everytime the cookie has been validated by the cookie middleware,
                    // so basically during every authenticated request
                    // the decryption of the cookie has already happened so we have access to the user claims
                    // and cookie properties - expiration, etc..
                    OnValidatePrincipal = async x =>
                    {
                        var expiresAt = DateTime.Parse(x.Properties.GetTokenValue("expires_at").Replace("+00:00", ""));

                        if (DateTime.UtcNow > expiresAt)
                        {
                            //Access token has expired so try to get new one with refresh token.
                            var accessToken = x.Properties.GetTokenValue("access_token");
                            var refreshToken = x.Properties.GetTokenValue("refresh_token");

                            HttpClient client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token/");
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var encoding = Encoding.UTF8;
                            string base64string = Convert.ToBase64String(encoding.GetBytes(Configuration["Spotify:ClientId"] + ":" + Configuration["Spotify:ClientSecret"]));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64string);
                            request.Content = new StringContent("grant_type=refresh_token&refresh_token=" + refreshToken, Encoding.UTF8, "application/x-www-form-urlencoded");
                            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                dynamic data = JObject.Parse(json);
                                x.Properties.UpdateTokenValue("access_token", (string)data.access_token);
                                var expires_at = DateTime.Now.AddSeconds(int.Parse((string)data.expires_in)).ToString();
                                x.Properties.UpdateTokenValue("expires_at", expires_at);
                                x.Properties.UpdateTokenValue("scope", (string)data.scope);
                            }
                            else
                            {
                                x.RejectPrincipal();
                            }
                        }
                    }
                };
            })
        .AddOAuth("Spotify", options =>
        {
            options.ClientId = Configuration["Spotify:ClientId"];
            options.ClientSecret = Configuration["Spotify:ClientSecret"];
            options.Scope.Add("user-top-read");
            options.CallbackPath = new PathString("/callback");

            options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
            options.TokenEndpoint = "https://accounts.spotify.com/api/token";
            options.UserInformationEndpoint = "https://api.spotify.com/v1/me";

            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");
            options.ClaimActions.MapJsonSubKey("urn:spotify_url", "external_urls", "spotify");
            options.SaveTokens = true;
            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                    context.RunClaimActions(user);
                }
            };
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}