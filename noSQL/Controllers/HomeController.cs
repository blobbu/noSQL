using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noSQL.Models;
using noSQL.Common;
using noSQL.Filters;
using noSQL.Helpers;

namespace noSQL.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cart()
        {
            CartModel model = new CartModel();
            model.Movies = new List<MovieCart>();

            string actualCart = redisGetValue(this.HttpContext.Session.Id, 2);
            var moviesId = actualCart.Split(";");

            foreach (var movieId in moviesId)
            {
                MovieCart movie = new HttpRequestHelper().getMovieToCart(movieId);
                model.Movies.Add(movie);
            }

            return View(model);
        }

        public void AddToCart(int id)
        {
            string actualCart = redisGetValue(this.HttpContext.Session.Id,2);
            actualCart = actualCart + ";" + id;
            redisSetKey(this.HttpContext.Session.Id, actualCart, 2);
        }

        public IActionResult News()
        {
            var model = getMoviesToNews();
            return View(model);
        }

        public IActionResult VideoSearch(VideoSearchModel model)
        {
            if (model != null)
            {
                model.Movies = new HttpRequestHelper().GetMovies(model.SearchByTitle);
                model.SearchByTitle = null;
            }
            return View(model);
        }

        public IActionResult LayoutUserName()
        {
            return View();
        }
        public NewsModel getMoviesToNews()
        {
            NewsModel model = new NewsModel { MoviesList = new List<Movies_ttl>() };
            var cass = new CassandraDatabase();
            Cassandra.RowSet movies = cass.getAllDataFromTable("movies_ttl2");
            
            foreach(var movie in movies)
            {
                Movies_ttl tmpMovie = new Movies_ttl
                {
                    Id = movie["id"].ToString(),
                    Title = movie["title"].ToString(),
                    Date = movie["release_date"].ToString(),
                    Genere = movie["genere"].ToString(),
                    Runtime = movie["runtime"].ToString(),
                    Rating = movie["rating"].ToString(),
                    Summary = movie["summary"].ToString(),
                    RawImg = movie["img_base64"].ToString(),
                    Price = movie["price"].ToString()
                };
                model.MoviesList.Add(tmpMovie);
            }
            return model;
        }
    }
}
