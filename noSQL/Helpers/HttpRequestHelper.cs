using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using noSQL.Models;

namespace noSQL.Helpers
{
    public class HttpRequestHelper
    {

        public MovieCart getMovieToCart(string id)
        {
            var movie = SendRequestForCart(id);
            return movie;
        }


        private string SendRequest(string title)
        {
            var url = "http://192.168.8.101:9200/movie/_search";
            var jsonData = "{ " +
                "\"query\":{" +
                    "\"match\": {" +
                        "\"Title\": {" +
                            "\"query\": \""+title+"\",\"fuzziness\": 1,\"prefix_length\": 1" +
                            "}" +
                        "}" +
                    "}" +
                "}";

            using (var client = new WebClient())
            {
                client.Headers.Add("content-type", "application/json");
                client.Credentials = new NetworkCredential("elastic", "phrsVCKCJIvLuDR87GDs");
                var jsonResponse = client.UploadString(url, jsonData);
                return jsonResponse;
            }
        }


        private MovieCart SendRequestForCart(string id)
        {   
            var url = "http://192.168.8.101:9200/movie/_search";
            var jsonData = "{ " +
                "\"query\":{" +
                    "\"match\": {" +
                        "\"MovieId\": " + id  +
                        "}" +
                    "}" +
                "}";

            using (var client = new WebClient())
            {
                client.Headers.Add("content-type", "application/json");
                client.Credentials = new NetworkCredential("elastic", "phrsVCKCJIvLuDR87GDs");
                var jsonResponse = client.UploadString(url, jsonData);

                var parsedObject = JObject.Parse(jsonResponse);
                var tmp = parsedObject["hits"]["hits"];

                string jsonMovie = tmp["_source"].ToString();

                MovieCart movie = JsonConvert.DeserializeObject<MovieCart>(jsonMovie);

                return movie;
            }
        }



        public List<Movie> GetMovies(string title)
        {
            List<Movie> movies = new List<Movie>();
            var jsonResult = SendRequest(title);

            var parsedObject = JObject.Parse(jsonResult);
            var tmp = parsedObject["hits"]["hits"];

            foreach (var tmpMovie in tmp)
            {
                string jsonMovie = tmpMovie["_source"].ToString();

                Movie movie = JsonConvert.DeserializeObject<Movie>(jsonMovie);
                movies.Add(movie);
            }

            return movies;
        }
    }
}