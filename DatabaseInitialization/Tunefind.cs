using Model.Tunefind;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInitialization
{
    public class Tunefind
    {
        public TunefindResponse GetSoundtracks(int tmdbId)
        {
            var apiRequest = WebRequest.Create("https://8576d087.api.tunefind.com/api/v2/movie/" + tmdbId + "?id-type=tmdb");
            string username = "8576d08726a03a9b15188dccda93f40d";
            string password = "442014c5446f25621a9c92ace89f61c2";
            string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            apiRequest.Headers.Add("Authorization", "Basic " + encoded);

            var apiResponse = Api.CallApi(apiRequest);

            if (apiResponse != null)
            {
                return JsonConvert.DeserializeObject<TunefindResponse>(apiResponse);
            }

            return null;
        }
    }
}
