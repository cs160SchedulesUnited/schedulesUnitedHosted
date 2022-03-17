using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace schedulesUnitedHosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        //TODO: getAllSurvey(string person), getOneSurvey(string id), getUserResps(string user, string id)
        //TODO: create helper method getAllResps(string id)

        //TODO: createOne(), editOne(string id, User person), deleteOne(string id, User person)

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
