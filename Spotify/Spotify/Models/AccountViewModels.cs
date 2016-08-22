using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Spotify.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        public SpotifyUser SpotifyUser { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

}
