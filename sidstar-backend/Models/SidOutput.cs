namespace sidstar_backend.Models
{
    public class SidOutput
    {
        public string airportName { get; set; }
        public List<KeyValuePair<string, int>> TopWaypointCounts { get; set; }
    }
}
