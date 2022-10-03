using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Airport
    {
        public string uid { set; get; }
        public string name { set; get; }
        public string icao { set; get; }
        public double lat { set; get; }
        public double lng { set; get; }
        public double alt { set; get; }
        public int iata { set; get; }

    }
}
