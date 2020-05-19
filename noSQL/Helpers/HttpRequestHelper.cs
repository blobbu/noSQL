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
        private string SendRequest(string title)
        {
            var url = "http://13.82.22.244:9200/movie/_search";
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