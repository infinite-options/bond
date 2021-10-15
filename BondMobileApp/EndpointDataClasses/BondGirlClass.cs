using System;
namespace BondMobileApp.EndpointDataClasses
{
    public class BondGirlClass
    {
        // Attributes of BondGirlClass                                          // using https://jsonutils.com/
        public string girl_uid { get; set; }
        public string girl_movie_uid { get; set; }
        public string bond_girl { get; set; }
        public string bond_girl_actress { get; set; }
        public string femme_fatale { get; set; }
        public string femme_fatale_actress { get; set; }
        public string movie_uid { get; set; }
        public string movie_title { get; set; }
        public int    movie_order { get; set; }
        public int    movie_year { get; set; }
        public int?   book_order { get; set; }
        public int?   book_year { get; set; }
        public string bond_actor { get; set; }
        public string director { get; set; }
        public string car { get; set; }
        public string gun { get; set; }

        // Constructor
        public BondGirlClass()
        {
        }
    }
}
