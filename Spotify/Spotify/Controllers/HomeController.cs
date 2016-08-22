using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Spotify.Business;
using Spotify.Models;

namespace Spotify.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var userManager = HttpContext.GetOwinContext().GetUserManager<SpotifyUserManager>();
            //var user = userManager.FindByNameAsync(User.Identity.Name);
            //if (user.Result == null)
            //{
            //    return View();
            //}
            //var model = new SpotifyUser
            //{
            //    DisplayName = user.Result.DisplayName,
            //    SpotifyId = user.Result.SpotifyId,
            //    SpotifyEmail = user.Result.SpotifyEmail,
            //    ExternalUrls = user.Result.ExternalUrls,
            //    Followers = user.Result.Followers,
            //    Href = user.Result.Href,
            //    Images = user.Result.Images,
            //    Product = user.Result.Product,
            //    Type = user.Result.Type,
            //    Uri = user.Result.Uri
            //};

            return View();
        }

        public ActionResult SearchResult()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchResult(string query, int? limit, int? offset, string type, string year, string genre, string tag)
        {
            var apiClient = new SpotifyApiClient();
            var model = new SpotifyApiResponseViewModel();
	        var qyear = !string.IsNullOrWhiteSpace(year) ? year : String.Empty;
	        var qgenre = !string.IsNullOrWhiteSpace(genre) ? genre : String.Empty;
	        var qtag = !string.IsNullOrWhiteSpace(tag) ? tag : String.Empty;
            switch (type)
            {
                case "Artists":
                    model.SearchedArtist = await apiClient.SearchArtistsAsync(query, limit, offset, qyear, qgenre, qtag);
                    var relatedIds = model.SearchedArtist.Artists.Items.Select(a => a.Id).ToList();
                    model.RelatedArtists = await apiClient.SearchRelatedArtistsAsync(relatedIds.FirstOrDefault());
                    break;
                case "Tracks":
                    model.SearchedTrack = await apiClient.SearchTracksAsync(query, limit, offset, qyear, qgenre, qtag);
                    break;
            }


            return View(model);
        }

        public string GetUserName()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<SpotifyUserManager>();
            var user = userManager.FindByNameAsync(User.Identity.Name);
            return user != null ? user.Result.UserName : string.Empty;
        }
    }
}