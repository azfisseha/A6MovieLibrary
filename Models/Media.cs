using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Models
{
    public abstract class Media
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public abstract string Display();
    }


}
