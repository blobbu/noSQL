using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Models
{
    public class Movies_ttl
    {
        public string Id { get; set; }
        public string Title { get; set;}
        public string Date { get; set; }
        public string Genere { get; set; }
        public string Runtime { get; set; }
        public string Rating { get; set;}
        public string Summary { get; set; }
        public string RawImg { get; set; }
        public string Price { get; set; }
    }
    public class NewsModel
    {
        public List<Movies_ttl> MoviesList;
    }
}
