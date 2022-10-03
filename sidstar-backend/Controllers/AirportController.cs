
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Deserializers;

using Models;
using sidstar_backend.Models;

namespace sidstar_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        [HttpPost]
        //get all airports
        public List<Airport> Post(Authetication_ authetication)
        {
            var client = new RestClient("https://open-atms.airlab.aero/api/v1/airac/airports");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("api-key", authetication.authetication);
            IRestResponse response = client.Execute(request);


            var deserialize = new JsonDeserializer();

            //list of airports and its data
            List<Airport> AirportList = deserialize.Deserialize<List<Airport>>(response);

            return AirportList;
        }

        

    }
}
