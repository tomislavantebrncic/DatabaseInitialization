using Model;
using Model.MovieDetails;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInitialization
{
    class Program
    {
        private static readonly string _connectionString = "mongodb://localhost:27017";
        private static readonly string _databaseName = "dm-mowizz";
        private static readonly string _moviesCollection = "movies";
        private static readonly string _artistsCollection = "artists";
        private static readonly string _genresCollection = "genres";
        private static readonly string _tmdbApiKey = "4c769c0e240a8d828da99a1019da67a8";
        private static readonly string _omdbApiKey = "2fc4648d";
        private static readonly string _lastFMApiKey = "72af7dc96ad4715e979e0fd048e07179";
        
        static void Main(string[] args)
        {
            MongoClient client = CreateClient();
            IMongoDatabase db = client.GetDatabase(_databaseName);

            IMongoCollection<TMDBAPIResponse> movieCollection = db.GetCollection<TMDBAPIResponse>(_moviesCollection);
            IMongoCollection<Id> ids = db.GetCollection<Id>("ids");

            Tunefind tf = new Tunefind();
      
            FreeTrialInitializationTunefind(movieCollection);

            InitializeGenres(db.GetCollection<Genre>(_genresCollection));

            var latestFound = ids.Find(x => true).FirstOrDefault();
            var latestId = (latestFound == null) ? 0 : latestFound.id;

            var latestIdReal = GetLatestIdFromTmdb();

            for (int i = latestId + 1; i < latestIdReal; i++)
            {
                var movieDetails = FetchMovieDetailsById(i);

                if (ids.CountDocuments(x => true) == 1)
                {
                    ids.FindOneAndDelete(x => true);
                }
                ids.InsertOne(new Id(i));

                if (movieDetails != null && !String.IsNullOrEmpty(movieDetails.release_date) && movieDetails.original_language.Equals("en"))
                {
                    if (movieDetails.runtime == null)
                    {
                        movieDetails.runtime = 0;
                    }
                    movieDetails.Ratings = GetRatingsFromImdbId(movieDetails.imdb_id);
                    movieDetails.Soundtrack = GetSoundtrackFromTunefind(movieDetails.id, tf);
                    SafeInsert(movieDetails, movieCollection);
                }
            }
        }

        private static void InitializeGenres(IMongoCollection<Genre> mongoCollection)
        {
            if (mongoCollection.CountDocuments(g => true) == 0)
            {
                var apiResponse = Api.CallApi("https://api.themoviedb.org/3/genre/movie/list?language=en-US&api_key=" + _tmdbApiKey);
                if (apiResponse != null)
                {
                    var genres = JsonConvert.DeserializeObject<GenreResponse>(apiResponse);

                    mongoCollection.InsertMany(genres.genres);
                }
            }
        }

        private static List<Song> GetSoundtrackFromTunefind(int id, Tunefind tf)
        {
            List<Song> soundtrack = new List<Song>();
            var response = tf.GetSoundtracks(id);

            if (response != null)
            {
                foreach (var song in response.songs)
                {
                    soundtrack.Add(new Song
                    {
                        Id = song.id,
                        Name = song.name,
                        TunefindUrl = song.tunefind_url,
                        Artist = new Artist
                        {
                            Id = song.artist.id,
                            Name = song.artist.name,
                            TunefindUrl = song.artist.tunefind_url
                        }
                    });
                }
            }

            return soundtrack;
        }

        private static TMDBAPIResponse FetchMovieDetailsById(int id)
        {
            var apiCall = "https://api.themoviedb.org/3/movie/" + id + "?api_key=" + _tmdbApiKey + "&append_to_response=keywords,credits,similar,images,videos";

            var apiResponse = Api.CallApi(apiCall);
            if (apiResponse != null)
            {
                return JsonConvert.DeserializeObject<TMDBAPIResponse>(apiResponse);
            }

            return null;
        }

        private static int GetLatestIdFromTmdb()
        {
            var apiCall = "https://api.themoviedb.org/3/movie/latest?api_key=" + _tmdbApiKey;

            var apiResponse = Api.CallApi(apiCall);
            return JsonConvert.DeserializeObject<TMDBAPIResponse>(apiResponse).id;
        }

        private static List<Rating> GetRatingsFromImdbId(string imdbId)
        {
            var apiCall = "http://www.omdbapi.com/?apikey=" + _omdbApiKey + "&i=" + imdbId;

            var apiResponse = Api.CallApi(apiCall);
            if (apiResponse != null)
            {
                return JsonConvert.DeserializeObject<OMDBMovieInfo>(apiResponse).Ratings;
            }

            return new List<Rating>();
        }

        private static MongoClient CreateClient()
        {
            return new MongoClient(_connectionString);
        }

        private static void FreeTrialInitializationTunefind(IMongoCollection<TMDBAPIResponse> movieCollection)
        {
            int[] ids = new int[] { 431530, 400106, 483104, 459954, 446791, 436459, 486859, 301337, 353486, 141052 };
            for (int i = 0; i < ids.Length; i++)
            {
                var movieDetails = FetchMovieDetailsById(ids[i]);
                movieDetails.Ratings = GetRatingsFromImdbId(movieDetails.imdb_id);
                movieDetails.Soundtrack = GetSoundtrackFromTunefind(movieDetails.id, new Tunefind());
                SafeInsert(movieDetails, movieCollection);
            }
        }

        private static void SafeInsert(TMDBAPIResponse movieDetails, IMongoCollection<TMDBAPIResponse> movieCollection)
        {
            try
            {
                movieCollection.InsertOne(movieDetails);
                Console.WriteLine("Inserted into db: [" + movieDetails.id + ", " + movieDetails.title + "] " + movieDetails.original_language);
            }
            catch (Exception e)
            {
                Console.WriteLine("Already in db: " + movieDetails.title);
            }
        }

        private class OMDBMovieInfo
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Rated { get; set; }
            public string Released { get; set; }
            public string Runtime { get; set; }
            public string Genre { get; set; }
            public string Director { get; set; }
            public string Writer { get; set; }
            public string Actors { get; set; }
            public string Plot { get; set; }
            public string Language { get; set; }
            public string Country { get; set; }
            public string Awards { get; set; }
            public string Poster { get; set; }
            public List<Rating> Ratings { get; set; }
            public string Metascore { get; set; }
            public string imdbRating { get; set; }
            public string imdbVotes { get; set; }
            public string imdbID { get; set; }
            public string Type { get; set; }
            public string DVD { get; set; }
            public string BoxOffice { get; set; }
            public string Production { get; set; }
            public string Website { get; set; }
            public string Response { get; set; }
        }
    }
}
