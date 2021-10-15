﻿using System;
namespace BondMobileApp.EndpointDataClasses
{
    public class VillainsClass
    {
        // Attributes of VillainsClass                                          // using https://jsonutils.com/
        public string villain_uid { get; set; }
        public string villain_movie_uid { get; set; }
        public string villain { get; set; }
        public string villain_actor { get; set; }
        public string objective { get; set; }
        public string outcome { get; set; }
        public string fate { get; set; }
        public string movie_uid { get; set; }
        public string movie_title { get; set; }
        public int movie_order { get; set; }
        public int movie_year { get; set; }
        public int? book_order { get; set; }
        public int? book_year { get; set; }
        public string bond_actor { get; set; }
        public string director { get; set; }
        public string car { get; set; }
        public string gun { get; set; }

        // Constructor
        public VillainsClass()
        {
        }
    }
}
