using Model.TraktTv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInitialization
{
    public class TraktTV
    {
        public List<TraktTVResponse> GetTraktTVResponse(string imdbId)
        {
            var apiRequest = WebRequest.Create("https://api.trakt.tv/movies/" + imdbId + "/related");
            apiRequest.Headers["trakt-api-key"] = "da9d164c7e5fb1d8b7039b11fbaa4f4ea6fed3175f326e98a085f4e3ddd7506e";
            apiRequest.Headers["trakt-api.version"] = "2";
            apiRequest.ContentType = "application/json";

            var apiResponse = Api.CallApi(apiRequest);

            if (apiResponse != null)
            {
                return JsonConvert.DeserializeObject<List<TraktTVResponse>>(apiResponse);
            }

            return null;
            
        }
    }
}
