using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noSQL.Models;
using noSQL.Common;
using noSQL.Filters;

namespace noSQL.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("News", "Home");
            //return View();
        }

        public IActionResult News()
        {
            var model = getMoviesToNews();
            return View(model);
        }

        [Authorize]
        public IActionResult VideoSearch()
        {
            //new Elastic().EsClient();
            return View();
        }

        public IActionResult LayoutUserName()
        {
            return View();
        }
        public NewsModel getMoviesToNews()
        {
            NewsModel model = new NewsModel { MoviesList = new List<Movies_ttl>() };
            var cass = new CassandraDatabase();
            Cassandra.RowSet movies = cass.getAllDataFromTable("movies_ttl");
            
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
                    PathImg = movie["path_img"].ToString(),
                    Price = movie["price"].ToString()
                };
                model.MoviesList.Add(tmpMovie);
            }
            return model;
        }
    }
}
