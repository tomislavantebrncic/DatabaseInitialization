using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MovieDetails
{
    public class MovieImages
    {
        public int id { get; set; }
        public ImageFile[] posters { get; set; }
        public ImageFile[] backdrops { get; set; }
    }

    public class ImageFile
    {
        public string file_path { get; set; }
        public string aspect_ratio { get; set; }
        public int height { get; set; }
        public string iso_639_1 { get; set; }
        public string vote_average { get; set; }
        public int width { get; set; }
        public int vote_count { get; set; }
    }
}
