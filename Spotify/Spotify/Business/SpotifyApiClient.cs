using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Spotify.Models;

namespace Spotify.Business
{
    public class SpotifyApiClient
    {
        protected const string ApiUrl = "https://api.spotify.com/v1";
        protected static readonly string ClientId = ConfigurationManager.AppSettings["spotify_clientId"];
        protected static readonly string ClientSecret = ConfigurationManager.AppSettings["spotify_clientSecret"];

        private static HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return httpClient;
        }

        private static string GetAccessToken(string userName)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<SpotifyUserManager>();
            var user = userManager.FindByNameAsync(userName);
            return user != null ? user.Result.Token : string.Empty;
        }

        public async Task<SearchArtistResponse> SearchArtistsAsync(string artistName, int? limit, int? offset, string year, string genre, string tag)
        {
            var httpClient = GetHttpClient();

			var qyear = !string.IsNullOrWhiteSpace(year) ? $"%20year:{year}" : null;
			var qgenre = !string.IsNullOrWhiteSpace(genre) ? $"%20genre:{genre}" : null;
			var qtag = !string.IsNullOrWhiteSpace(tag) ? $"%20tag:{tag}" : null;

			var sb = new StringBuilder();
			sb.Append(ApiUrl);
			sb.Append($"/search?q={artistName}{qyear}{qgenre}{qtag}");

			if (offset > 0)
            {
                sb.Append($"&offset={offset}");
            }
            if (limit > 0)
            {
                sb.Append($"&limit={limit}");
            }

	        sb.Append("&type=artist");

            var query = sb.ToString();
            var response = await httpClient.GetAsync(query);
            var spotifyContent = response.Content;
            var spotifyResponse =
                JsonConvert.DeserializeObject<SearchArtistResponse>(await spotifyContent.ReadAsStringAsync());

            return spotifyResponse;
        }

        public async Task<SearchTrackResponse> SearchTracksAsync(string trackName, int? limit, int? offset, string year, string genre, string tag)
        {
            var httpClient = GetHttpClient();

			var qyear = !string.IsNullOrWhiteSpace(year) ? $"%20year:{year}" : null;
			var qgenre = !string.IsNullOrWhiteSpace(genre) ? $"%20genre:{genre}" : null;
			var qtag = !string.IsNullOrWhiteSpace(tag) ? $"%20tag:{tag}" : null;

			var sb = new StringBuilder();
            sb.Append(ApiUrl);
            sb.Append($"/search?q={trackName}{qyear}{qgenre}{qtag}");

			
			if (offset > 0)
			{
				sb.Append($"&offset={offset}");
			}
			if (limit > 0)
			{
				sb.Append($"&limit={limit}");
			}

	        sb.Append("&type=track");

			var query = sb.ToString();
            var response = await httpClient.GetAsync(query);
            var spotifyContent = response.Content;
            var spotifyResponse =
                JsonConvert.DeserializeObject<SearchTrackResponse>(await spotifyContent.ReadAsStringAsync());

            return spotifyResponse;
        }

        public async Task<RelatedArtistsCollection> SearchRelatedArtistsAsync(string artistId)
        {
            var httpClient = GetHttpClient();
            var sb = new StringBuilder();
            sb.Append(ApiUrl);
            sb.Append($"/artists/{artistId}/related-artists");
            var query = sb.ToString();
            var response = await httpClient.GetAsync(query);
            var spotifyContent = response.Content;
            var spotifyResponse =
                JsonConvert.DeserializeObject<RelatedArtistsCollection>(await spotifyContent.ReadAsStringAsync());
            return spotifyResponse;
        }

        public async Task<SearchArtistResponse> GetSelfTopAsync(string userName, string type)
        {
    
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                GetAccessToken(userName));
            var sb = new StringBuilder();
            sb.Append(ApiUrl);
            sb.Append($"/me/top/{type}");
            var query = sb.ToString();
            var response = await httpClient.GetAsync(query);

			/*'UTF8' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.
				Parameter name: name*/
            var spotifyResponse = JsonConvert.DeserializeObject<SearchArtistResponse>(await response.Content.ReadAsStringAsync());
            return spotifyResponse;
        }
    }
}
 