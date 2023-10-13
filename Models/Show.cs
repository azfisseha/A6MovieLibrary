using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Models
{
    public class Show : Media
    {
        public int Season { get; set; }
        public int Episode { get; set; }
        public string Writers { get; set; }
        public override string Display()
        {
            return $"{Id,-5} | {Title,-80} | {Season,-8} | {Episode,-12} | {Writers,-8}";
        }
    }

    public class ShowMap : ClassMap<Show>
    {
        public ShowMap() 
        {
            Map(s => s.Id).Index(0).Name("showId");
            Map(s => s.Title).Index(1).Name("title");
            Map(s => s.Season).Index(2).Name("season");
            Map(s => s.Episode).Index(3).Name("episode");
            Map(s => s.Writers).Index(4).Name("writer");
        }
    }
}
