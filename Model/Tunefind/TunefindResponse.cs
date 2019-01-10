using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tunefind
{
    public class Artist
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime date_updated { get; set; }
        public string tunefind_url { get; set; }
        public string tunefind_api_url { get; set; }
    }

    public class Store
    {
        public string id { get; set; }
        public List<string> countries { get; set; }
        public string url { get; set; }
    }

    public class Appearance
    {
        public int song_id { get; set; }
        public string song_name { get; set; }
        public string artist_id { get; set; }
        public string artist_name { get; set; }
        public DateTime date_created { get; set; }
        public DateTime air_date { get; set; }
        public DateTime syndication_date { get; set; }
    }

    public class Song
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date_updated { get; set; }
        public string tunefind_url { get; set; }
        public string tunefind_api_url { get; set; }
        public Artist artist { get; set; }
        public List<Store> stores { get; set; }
        public Appearance appearance { get; set; }
    }

    public class TunefindResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string tunefind_url { get; set; }
        public string tunefind_api_url { get; set; }
        public DateTime air_date { get; set; }
        public List<Song> songs { get; set; }
    }
}
