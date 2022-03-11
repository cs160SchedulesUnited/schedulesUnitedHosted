using schedulesUnitedHosted.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using schedulesUnitedHosted.Server;

namespace schedulesUnitedHosted.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBController : ControllerBase
    {

        // GET: <DBController>
        [HttpGet]
        public String Get()
        {
            //TODO: Make this an env variable if possible, our connection string should not be publicly visible
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                MySqlCommand test = new MySqlCommand("");
                con.Close();
            }
            return "Connection Successful";
        } 

        // GET api/<DBController>/5
        [HttpGet("{id}")]
        public Survey Get(int id)
        {
            //TODO: Define variables to store query data
            //TODO: Create a DB query for event id $id
            //TODO: Create a survey object to return to front end that stores the query data
            var request = new Survey(/* Query Data */);
            return request;
        }

        // POST api/<DBController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //TODO: Create behavior to accept posted data and submit a db query to add the response
        }
    }
}
