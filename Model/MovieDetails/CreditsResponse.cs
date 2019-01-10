using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MovieDetails
{
    public class CreditsResponse
    {
        public int id { get; set; }
        public CastPerson[] cast { get; set; }
        public Crew[] crew { get; set; }
    }

    public class CastPerson
    {
        public int cast_id { get; set; }
        public string character { get; set; }
        public string credit_id { get; set; }
        public int gender { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public string profile_path { get; set; }

    }

 
}
