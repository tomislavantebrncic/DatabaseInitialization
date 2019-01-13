using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TraktTv
{
  
    public class Ids
    {
        public int trakt { get; set; }
        public string slug { get; set; }
        public string imdb { get; set; }
        public int tmdb { get; set; }
    }

    public class TraktTVResponse
    {
        public string title { get; set; }
        public int year { get; set; }
        public Ids ids { get; set; }
    }

}
