using System;
namespace BondMobileApp.EndpointDataClasses
{
    public class MovieClass
    {
        // Attributes of MovieClass                                          // using https://jsonutils.com/
        public string movie_uid { get; set; }
        public string movie_title { get; set; }
        public int    movie_order { get; set; }
        public string movie_year { get; set; }
        public int    book_order { get; set; }
        public string book_year { get; set; }
        public string bond_actor { get; set; }
        public string director { get; set; }
        public string M { get; set; }
        public string Q { get; set; }
        public string Moneypenny { get; set; }
        public string car { get; set; }
        public string gun { get; set; }
        public string girl_uid { get; set; }
        public string girl_movie_uid { get; set; }
        public string bond_girl { get; set; }
        public string bond_girl_actress { get; set; }
        public string femme_fatale { get; set; }
        public string femme_fatale_actress { get; set; }
        public string song_uid { get; set; }
        public string song_movie_uid { get; set; }
        public int?   song_year { get; set; }
        public string score_composer { get; set; }
        public string title_song { get; set; }
        public string title_song_composer { get; set; }
        public string title_song_performer { get; set; }
        public string uk_peak { get; set; }
        public string us_peak { get; set; }

        // Constructor
        public MovieClass()
        {
        }
    }
}
