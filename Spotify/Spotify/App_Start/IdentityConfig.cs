using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Spotify.Models;

namespace Spotify
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class SpotifyUserManager : UserManager<SpotifyUser>
    {
        public SpotifyUserManager(IUserStore<SpotifyUser> store)
            : base(store)
        {
        }

        public static SpotifyUserManager Create(IdentityFactoryOptions<SpotifyUserManager> options, IOwinContext context)
        {
            var manager = new SpotifyUserManager(new UserStore<SpotifyUser>(context.Get<SpotifyDbContext>()));

            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class SpotifySignInManager : SignInManager<SpotifyUser, string>
    {
        public SpotifySignInManager(SpotifyUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SpotifyUser user)
        {
            return user.GenerateUserIdentityAsync((SpotifyUserManager) UserManager);
        }

        public static SpotifySignInManager Create(IdentityFactoryOptions<SpotifySignInManager> options,
            IOwinContext context)
        {
            return new SpotifySignInManager(context.GetUserManager<SpotifyUserManager>(), context.Authentication);
        }
    }
}