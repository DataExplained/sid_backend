using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SID
    {
        public string name { get; set; }
        public airport_ airport { get; set; }
        public List<waypoints> waypoints { get; set; }
    }
}
