using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInitialization
{
    public class Api
    {
        public static string CallApi(string apiCall)
        {
            var apiRequest = WebRequest.Create(apiCall);

            return CallApi(apiRequest);
        }

        public static string CallApi(WebRequest apiRequest)
        {
            try
            {
                using (var response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                return null;
            }
        }
    }
}
