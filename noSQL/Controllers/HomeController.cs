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

        public IActionResult AddToCart( int id)
        {
            string actualCart = redisGetValue(this.HttpContext.Session.Id,2);

            if (actualCart is null)
                actualCart =  id.ToString();
            else      
                actualCart = actualCart + ";" + id;
            redisSetKeyWithExpire(this.HttpContext.Session.Id, actualCart, 30*60, 2);
            return RedirectToAction("Cart", "Home");
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
                if (model.SearchByTitle != null)
                {
                    model.Movies = new HttpRequestHelper().GetMovies(model.SearchByTitle);
                    model.SearchByTitle = null;
                    model.SearchByRating = null;
                }
                else if (model.SearchByRating != null)
                {
                    model.Movies = new HttpRequestHelper().GetFilesByRaiting(model.SearchByRating);
                    model.SearchByTitle = null;
                    model.SearchByRating = null;
                }
            }
            if (model.Movies == null) model.Movies = new List<Movie>();
            
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
        public IActionResult SubmitCart(CartModel model)
        {
            CartModel oldmodel = new CartModel();
            oldmodel.Movies = new List<MovieCart>();

            string actualCart = redisGetValue(this.HttpContext.Session.Id, 2);
            var moviesId = actualCart.Split(";");

            foreach (var movieId in moviesId)
            {
                MovieCart movie = new HttpRequestHelper().getMovieToCart(movieId);
                oldmodel.Movies.Add(movie);
            }
            model = oldmodel;

            MongoDatabase mongoDb = new MongoDatabase("mongodb://root:root@192.168.8.101:27017");
            mongoDb.setDatabase("noSQL");
            mongoDb.setCollection("order");
            double totalPrice = 0;
            List<string> movies = new List<string>();
            string user = this.CurrentUser.Name;

            foreach (var movie in model.Movies)
            {
                movies.Add(movie.Title);
                try
                {
                    totalPrice += Convert.ToDouble(movie.Price);
                }
                catch
                {
                    continue;
                }
            }
            mongoDb.AddOrder(user, totalPrice.ToString(), movies.ToArray());
            CassandraNotification();
            redisDeleteKey(this.HttpContext.Session.Id, 2);
            return View("VideoSearch");
        }
        public void CassandraNotification()
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

            double totalPrice = 0.0;
            foreach (var movie in model.Movies)
            {
                try
                {
                    totalPrice += Convert.ToDouble(movie.Price);
                }
                catch
                {
                    continue;
                }
            }

            var cass = new CassandraDatabase();
            cass.insertNotification(this.CurrentUser.Name, totalPrice.ToString());
        }
        public IActionResult Notification()
        {
            NotificationModel model = new NotificationModel();
            model.Notifications = new List<Notification>();
            var cass = new CassandraDatabase();
            string condition = "where user_login = " + "'" + this.CurrentUser.Name + "'";
            Cassandra.RowSet notifications = cass.getAllDataFromTableWithCondition("notification", condition);

            foreach (var notification in notifications)
            {
                Notification tmpNotification = new Notification
                {
                    User = notification["user_login"].ToString(),
                    Time = notification["added_time"].ToString().Split("+")[0],
                    Message = notification["message"].ToString(),

                };
                model.Notifications.Add(tmpNotification);
            }

            return View(model);
        }
    }
    
}
