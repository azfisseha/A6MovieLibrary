using CsvHelper;
using CsvHelper.Configuration.Attributes;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;                     
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MovieLibrary.Services;

/// <summary>
///     This concrete service and method only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public class FileService : IFileService
{
    //private readonly ILogger<IFileService> _logger;
    
    private readonly string _fileNameMovies;
    private readonly string _fileNameShows;
    private readonly string _fileNameVideos;

    public List<Movie> Movies { get; set; } = new();
    public List<Show> Shows { get; set; } = new();
    public List<Video> Videos { get; set; } = new();


    //public FileService(ILogger<IFileService> logger)
    public FileService()
    {
        //_logger = logger;
        _fileNameMovies = "Files/movies.csv";
        _fileNameShows = "Files/shows.csv";
        _fileNameVideos = "Files/videos.csv";

    }
    
    public void Display()
    {
        //Not happy with this solution...


        string subChoice;
        do
        {
            Console.WriteLine("1) Movies");
            Console.WriteLine("2) Shows");
            Console.WriteLine("3) Videos");
            Console.WriteLine("X) Previous menu");
            subChoice = Console.ReadLine();

            if (subChoice == "1")
            {
                if (Movies.Count == 0)
                {
                    Console.WriteLine("No movies to display");
                    return;
                }
                Console.WriteLine($"{"Id",-5} | {"Title",-80} | {"Genres",-10}");
                var entry = 0;

                foreach (var movie in Movies)
                {
                    var movies = Movies.Skip(entry).Take(10);

                    foreach (var m in movies)
                    {
                        Console.WriteLine(m.Display());
                        entry++;
                    }
                    movies = Movies.Skip(entry).Take(10);

                    if (Movies.Count < entry + 10 || !ContinueDisplaying())
                    {
                        break;
                    }
                }
                break;
            }
            else if (subChoice == "2")
            {
                if (Shows.Count == 0)
                {
                    Console.WriteLine("No shows to display");
                    return;
                }
                Console.WriteLine($"{"Id",-5} | {"Title",-80} | {"Season",-8} | {"Episode",-12} | {"Writers",-8}");
                var entry = 0;

                foreach (var show in Shows)
                {
                    var shows = Shows.Skip(entry).Take(10);

                    foreach (var s in shows)
                    {
                        Console.WriteLine(s.Display());
                        entry++;
                    }
                    shows = Shows.Skip(entry).Take(10);

                    if (Shows.Count < entry + 10 || !ContinueDisplaying())
                    {
                        break;
                    }
                }
                break;
            }
            else if (subChoice == "3")
            {
                if (Videos.Count == 0)
                {
                    Console.WriteLine("No videos to display");
                    return;
                }
                Console.WriteLine($"{"Id",-5} | {"Title",-80} | {"Format",-18} | {"Length",-8} | {"Regions",-8}");
                var entry = 0;

                foreach (var video in Videos)
                {
                    var videos = Videos.Skip(entry).Take(10);

                    foreach (var v in videos)
                    {
                        Console.WriteLine(v.Display());
                        entry++;
                    }
                    videos = Videos.Skip(entry).Take(10);

                    if (Videos.Count < entry + 10 || !ContinueDisplaying())
                    {
                        break;
                    }
                }
                break;
            }
        } while (subChoice != "X");

    }

    

    private bool ContinueDisplaying()
    {
        Console.WriteLine("Hit Enter to continue or ESC to cancel");
        var input = Console.ReadKey();
        while (input.Key != ConsoleKey.Enter && input.Key != ConsoleKey.Escape)
        {
            input = Console.ReadKey();
            Console.WriteLine("Hit Enter to continue or ESC to cancel");
        }

        if (input.Key == ConsoleKey.Escape)
        {
            return false;
        }

        return true;
    }

    public void Read()
    {
        if(!File.Exists(_fileNameShows))
        {
            //log error
            Console.WriteLine($"File {_fileNameShows} does not exist");
            return;
        }
        if (!File.Exists(_fileNameVideos))
        {
            //log error
            Console.WriteLine($"File {_fileNameVideos} does not exist");
            return;
        }
        if (!File.Exists(_fileNameMovies))
        {
            //log error
            Console.WriteLine($"File {_fileNameMovies} does not exist");
            return;
        }
        else
        {
            using (var sr = new StreamReader(_fileNameMovies))
            {
                using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                {

                    csv.Context.RegisterClassMap<MovieMap>();

                    var movie = new Movie();
                    var movieRecords = csv.GetRecords<Movie>();
                    Movies = movieRecords.ToList();

                }
            }

            using (var sr = new StreamReader(_fileNameShows))
            {
                using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<ShowMap>();

                    var show = new Show();
                    var showRecords = csv.GetRecords<Show>();
                    Shows = showRecords.ToList();
                }
            }
            using (var sr = new StreamReader(_fileNameVideos))
            {
                using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<VideoMap>();
                    var video = new Video();
                    var videoRecords = csv.GetRecords<Video>();
                    Videos = videoRecords.ToList();
                }
            }
        }
    }

    public void Write()
    {
        //_logger.Log(LogLevel.Information, "Writing");

        if (!File.Exists(_fileNameMovies))
        {
            //log error here
            Console.WriteLine($"{_fileNameMovies} does not exist");
            return;
        }
        if (Movies.Count == 0)
        {
            //Log error here
            Console.WriteLine("Read in file first before updating");
            return;
        }

        var movie = new Movie();

        Console.Write("Enter Title: ");
        movie.Title = Console.ReadLine();
        var identical = false;
        foreach (var record in Movies)
        {
            if (string.Equals(movie.Title, record.Title, StringComparison.InvariantCulture))
            {
                Console.WriteLine("Could not add movie entry - identical to existing entry");
                Console.WriteLine($"ID : {record.Id} | Title : {record.Title} | {string.Join(",", record.Genres)}");
                identical = true;
                break;
            }
        }

        if (!identical)
        {
            movie.Genres = new List<string>();
            var addtlGenres = true;
            do
            {
                Console.Write("Enter Genre (X when done): ");

                string input = Console.ReadLine();
                if (input.ToUpper() == "X")
                {
                    if (movie.Genres.Count == 0)
                    {
                        Console.WriteLine("Enter at least one Genre to continue");
                    }
                    else
                    {
                        addtlGenres = false;
                    }
                }
                else
                {
                    movie.Genres.Add(input);
                }
            } while (addtlGenres);


            movie.Id = Movies[Movies.Count - 1].Id + 1;
            Movies.Add(movie);
            using (var stream = new StreamWriter(_fileNameMovies, true))
            {
                stream.WriteLine($"{movie.Id},{movie.Title},{string.Join("|", movie.Genres)}");
                stream.Close();
            }
        }
    }
}
