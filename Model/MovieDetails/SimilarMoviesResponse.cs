using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MovieDetails
{
    public class SimilarMoviesResponse
    {
        public int page { get; set; }
        //[JsonProperty("results")]
        public SimilarMovie[] results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }

    public class SimilarMovie
    {
        public string vote_count { get; set; }
        public int id { get; set; }
        public string video { get; set; }
        public string title { get; set; }
        public string popularity { get; set; }
        public string poster_path { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public int[] genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public string vote_average { get; set; }

    }
}
