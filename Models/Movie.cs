using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Models
{

    //Using entity model from MovieRepository solution showed in class
    //Also using the CSVHelper package to help handle splitting and parsing the csv file.
    //Code here is directly from the solution.
    public class Movie : Media
    {
        public List<string> Genres { get; set; }
        public long Id {  get; set; }
        public string Title { get; set; }

        public override string Display()
        {
            return $"{Id,-5} | {Title,-80} | {string.Join(",", Genres),-10}";
        }
    }

    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Id).Index(0).Name("movieId");
            Map(m => m.Title).Index(1).Name("title");
            Map(m => m.Genres).Index(2).Name("genres").TypeConverter<GenresConverter<List<string>>>();
        }
    }

    public class GenresConverter<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var array = text.Split("|");

            return array.ToList();
        }

        // instructions found at https://joshclose.github.io/CsvHelper/examples/configuration/class-maps/type-conversion/
        public override string? ConvertToString(object? o, IWriterRow row, MemberMapData memberMapData)
        {
            var genres = (List<string>)o;

            return string.Join(",", genres);
        }
    }
}
