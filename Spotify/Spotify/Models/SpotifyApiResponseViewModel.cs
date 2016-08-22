namespace Spotify.Models
{
    public class SpotifyApiResponseViewModel
    {
        public SearchArtistResponse SearchedArtist { get; set; }
        public RelatedArtistsCollection RelatedArtists { get; set; }
        public SearchTrackResponse SearchedTrack { get; set; }
        public SearchArtistResponse UserTopCollection { get; set; }
    }
}