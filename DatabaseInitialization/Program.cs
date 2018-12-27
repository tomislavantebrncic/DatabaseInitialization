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
        private static readonly string _tmdbApiKey = "4c769c0e240a8d828da99a1019da67a8";
        private static readonly string _omdbApiKey = "2fc4648d";

        static void Main(string[] args)
        {
            //MongoClient client = CreateClient();
            //IMongoDatabase db = client.GetDatabase(_databaseName);

            //IMongoCollection<Movie> movieCollection = db.GetCollection<Movie>(_moviesCollection);
            //IMongoCollection<Id> ids = db.GetCollection<Id>("ids");

            //var latestFound = ids.Find(x => true).FirstOrDefault();
            //var latestId = (latestFound == null) ? 0 : latestFound.id;

            //var latestIdReal = GetLatestIdFromTmdb();

            //for (int i = latestId+1; i < latestIdReal; i++)
            //{
            //    var movie = FetchMovieById(i);

            //    if (ids.CountDocuments(x => true) == 1)
            //    {
            //        ids.FindOneAndDelete(x => true);
            //    }
            //    ids.InsertOne(new Id(i));

            //    if (movie != null)
            //    {
            //        movie.Ratings = GetRatingsFromImdbId(movie.imdb_id);
            //        Console.WriteLine("Inserting into db: [" + movie.id + ", " + movie.title + "]");
            //        movieCollection.InsertOne(movie);
            //    }
            //}
            MongoClient client = CreateClient();
            IMongoDatabase db = client.GetDatabase(_databaseName);

            IMongoCollection<TMDBAPIResponse> movieCollection = db.GetCollection<TMDBAPIResponse>(_moviesCollection);
            IMongoCollection<Id> ids = db.GetCollection<Id>("ids");

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

                if (movieDetails != null && !String.IsNullOrEmpty(movieDetails.release_date))
                {
                    if (movieDetails.runtime == null)
                    {
                        movieDetails.runtime = 0;
                    }
                    movieDetails.Ratings = GetRatingsFromImdbId(movieDetails.imdb_id);
                    Console.WriteLine("Inserting into db: [" + movieDetails.id + ", " + movieDetails.title + "]");
                    movieCollection.InsertOne(movieDetails);
                }
            }
        }

        private static TMDBAPIResponse FetchMovieDetailsById(int id)
        {
            var apiCall = "https://api.themoviedb.org/3/movie/" + id + "?api_key=" + _tmdbApiKey + "&append_to_response=keywords,credits,similar,images";

            var apiResponse = CallApi(apiCall);
            if (apiResponse != null)
            {
                return JsonConvert.DeserializeObject<TMDBAPIResponse>(apiResponse);
            }

            return null;
        }

        //private static Movie FetchMovieById(int id)
        //{
        //    var apiCall = "https://api.themoviedb.org/3/movie/"+id+"?api_key=" + _tmdbApiKey + "&append_to_response=keywords,credits";

        //    var apiResponse = CallApi(apiCall);
        //    if (apiResponse != null)
        //    {
        //        return JsonConvert.DeserializeObject<Movie>(apiResponse);
        //    }

        //    return null;
        //}

        private static int GetLatestIdFromTmdb()
        {
            var apiCall = "https://api.themoviedb.org/3/movie/latest?api_key=" + _tmdbApiKey;

            var apiResponse = CallApi(apiCall);
            return JsonConvert.DeserializeObject<TMDBAPIResponse>(apiResponse).id;
        }

        private static List<Rating> GetRatingsFromImdbId(string imdbId)
        {
            var apiCall = "http://www.omdbapi.com/?apikey=" + _omdbApiKey + "&i=" + imdbId;

            var apiResponse = CallApi(apiCall);
            if (apiResponse != null)
            {
                return JsonConvert.DeserializeObject<OMDBMovieInfo>(apiResponse).Ratings;
            }

            return new List<Rating>();
        }

        private static string CallApi(string apiCall)
        {
            var apiRequest = WebRequest.Create(apiCall);

            try
            {
                using (var response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();
                }
            } catch (WebException e)
            {
                return null;
            }
        }

        private static MongoClient CreateClient()
        {
            return new MongoClient(_connectionString);
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
