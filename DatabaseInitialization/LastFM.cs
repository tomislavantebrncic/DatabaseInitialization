using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInitialization
{
    public class LastFM
    {
        private readonly string _apiKey;

        public LastFM(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void GetArtist(string name)
        {
            var url = "http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist="+name+"&api_key="+_apiKey+"&format=json";
        }
    }
}
