using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Models
{
    public class Video : Media
    {
        public string Format { get; set; }
        public int Length { get; set; }
        public int[] Regions { get; set; }

        public override string Display()
        {
            return $"{Id,-5} | {Title,-80} | {Format,-18} | {Length,-8} | {string.Join(",", Regions), -8}";
        }

    }

    public class VideoMap : ClassMap<Video>
    {
        public VideoMap()
        {
            Map(v => v.Id).Index(0).Name("videoId");
            Map(v => v.Title).Index(1).Name("title");
            Map(v => v.Format).Index(2).Name("format");
            Map(v => v.Length).Index(3).Name("length");
            Map(v => v.Regions).Index(4).Name("regions").TypeConverter<RegionsConverter<int[]>>();
        }
    }

    public class RegionsConverter<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var array = text.Split("|").ToArray();
            var intArray = new int[array.Length];
            for(int i = 0; i < intArray.Length; i++) 
            {
                if (!Int32.TryParse(array[i], out int val))
                {
                    intArray[i] = 0;
                }
                else
                {
                    intArray[i] = val;
                }
            }
            return intArray;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var regions = (int[])value;
            return string.Join("|", regions);
        }
    }

}
