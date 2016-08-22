using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Spotify.Models
{
    public class SpotifyDbContext : IdentityDbContext<SpotifyUser>
    {
        public SpotifyDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static SpotifyDbContext Create()
        {
            return new SpotifyDbContext();
        }
    }

    public class SpotifyUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SpotifyUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public string Token { get; set; }

        public string BirthDate { get; set; }

        public string Country { get; set; }

        public string DisplayName { get; set; }

        public string SpotifyEmail { get; set; }

        public string ExternalUrls { get; set; }

        public string Followers { get; set; }

        public string Href { get; set; }

        public string SpotifyId { get; set; }

        public IList<Image> Images { get; set; }

        public string Product { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }
    }
}