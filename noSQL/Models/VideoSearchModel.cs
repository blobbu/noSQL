using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Models
{
    public class VideoSearchModel
    {
        public string SearchByTitle { get; set; }
        public List <Movie> Movies { get; set; }
    }
    public class Movie
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string MPAA_Rating { get; set; }
        public string Release_Date { get; set; }
        public string Genre { get; set; }
        public string Runtime { get; set; }
        public string Rating { get; set; }
        public string Summary { get; set; }
    }
}
