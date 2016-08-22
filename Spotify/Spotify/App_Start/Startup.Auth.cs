using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.Spotify;
using Owin.Security.Providers.Spotify.Provider;
using Spotify.Models;

namespace Spotify
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(SpotifyDbContext.Create);
            app.CreatePerOwinContext<SpotifyUserManager>(SpotifyUserManager.Create);
            app.CreatePerOwinContext<SpotifySignInManager>(SpotifySignInManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity =
                       SecurityStampValidator.OnValidateIdentity<SpotifyUserManager, SpotifyUser>(
                           TimeSpan.FromMinutes(30),
                           (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            //Spotify external login
            var spotifyOptions = new SpotifyAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["spotify_clientId"],
                ClientSecret = ConfigurationManager.AppSettings["spotify_clientSecret"],
                Provider = new SpotifyAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        const string xmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";

                        context.Identity.AddClaim(new Claim("urn:spotify:access_token", context.AccessToken,
                            xmlSchemaString, "Spotify"));
                        foreach (var x in context.User)
                        {
                            var claimType = $"urn:spotify:{x.Key}";
                            var claimValue = x.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                                context.Identity.AddClaim(new Claim(claimType, claimValue, xmlSchemaString, "Spotify"));
                        }
                        return Task.FromResult(0);
                    }
                },
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                Scope =
                {
                    "playlist-read-private",
                    "user-top-read",
                    "user-read-email",
                    "streaming",
                    "playlist-modify-public",
                    "user-follow-read",
                    "user-library-read"
                }
            };
            app.UseSpotifyAuthentication(spotifyOptions);
        }
    }
}