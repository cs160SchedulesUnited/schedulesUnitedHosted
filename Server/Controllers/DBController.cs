using schedulesUnitedHosted.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using schedulesUnitedHosted.Server;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace schedulesUnitedHosted.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBComController : ControllerBase
    {
        // GET: api/<DBController>
        [HttpGet]
        public String Test()
        {
            //TODO: Make this an env variable if possible, our connection string should not be publicly visible
            DBCon conGen = new DBCon("server=cs160-db.cocfzrdakcvx.us-west1.rds.amazonaws.com;port=3306;database=newschema;user=admin;password=CS160DBCon");
            MySqlConnection con = conGen.GetConnection();
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
        }
    }
}
