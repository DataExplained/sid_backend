using Microsoft.AspNetCore.Mvc;
using Models;
using RestSharp;
using RestSharp.Deserializers;
using sidstar_backend.Models;

namespace sidstar_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SIDController : Controller
    {
        
        [HttpPost]
        public SidOutput ForOneAirportPost(InputAirport AirportInput)
        {
            var client = new RestClient("https://open-atms.airlab.aero/api/v1/airac/sids/airport/" + AirportInput.AirportName);
            client.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("api-key", AirportInput.authetication);
            IRestResponse response = client.Execute(request1);

            var deserialize = new JsonDeserializer();
            //Console.WriteLine(response.Content);

            //list of airports and its data
            List<SID> SID_ = deserialize.Deserialize<List<SID>>(response);


            return getTop2waypoints(SID_, AirportInput.AirportName);
        }

        [HttpPost("AllAirports")]
        public List<SidOutput> ForAllAirportGet(Authetication_ authetication_)
        {
            List<string> ListOfAirportNames = new List<string>();
            ListOfAirportNames = getListAirports(authetication_.authetication);
            List<SidOutput> AllSidOutputs = new List<SidOutput>();

            foreach (string AirportName in ListOfAirportNames)
            {
                var client = new RestClient("https://open-atms.airlab.aero/api/v1/airac/sids/airport/" + AirportName);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("api-key", authetication_.authetication);
                IRestResponse response = client.Execute(request);

                var deserialize = new JsonDeserializer();
                //Console.WriteLine(response.Content);

                //list of airports and its data
                List<SID> SID_ = deserialize.Deserialize<List<SID>>(response);
                try
                {
                    SidOutput a = getTop2waypoints(SID_, AirportName);
                    AllSidOutputs.Add(a);

                }
                catch
                {
                    AllSidOutputs.Add(new SidOutput
                    {
                        airportName = AirportName,
                        TopWaypointCounts = null

                    }
                    );
                }
            }


            return AllSidOutputs;
        }

        private List<string> getListAirports(string key)
        {
            var client = new RestClient("https://open-atms.airlab.aero/api/v1/airac/airports");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("api-key", key);
            IRestResponse response = client.Execute(request);


            var deserialize = new JsonDeserializer();

            //list of airports and its data
            List<Airport> AirportList = deserialize.Deserialize<List<Airport>>(response);

            List<string> ListOfAirportNames = new List<string>();

            foreach (Airport airport in AirportList)
            {
                ListOfAirportNames.Add(airport.name);
            }

            return ListOfAirportNames;
        }

        private SidOutput getTop2waypoints(List<SID> SID_, string airportname)
        {
            //Console.WriteLine(SID_);
            Dictionary<string, int> waypointsDict = new Dictionary<string, int>();

            //Console.WriteLine(SID_);


            foreach (SID sid in SID_)
            {
                foreach (waypoints waypoint in sid.waypoints)
                {
                    //Console.Write(waypoint.uid);

                    if (!waypointsDict.ContainsKey(waypoint.uid))
                    {
                        waypointsDict.Add(waypoint.uid, 1);
                    }
                    else
                    {
                        waypointsDict[waypoint.uid]++;
                    }
                }
            }


            string keyOfMaxValue = waypointsDict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; // "a"

            var sortedDict = (from entry in waypointsDict orderby entry.Value descending select entry)
               .ToDictionary(pair => pair.Key, pair => pair.Value).Take(2);

            var Top2Waypoint = sortedDict.ToList();

            SidOutput Top2WaypointsList = new SidOutput();

            Top2WaypointsList.airportName = airportname;
            //Top2WaypointsList.TopWaypointCounts = Top2Waypoint;
            Top2WaypointsList.TopWaypointCounts = Top2Waypoint;


            return Top2WaypointsList;
        }
    }
}
