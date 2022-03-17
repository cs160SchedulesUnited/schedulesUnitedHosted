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
                using (var reader = test.ExecuteReader())
                    while (reader.Read())
                    {
                        //handle all the responses from the DB here
                    }
                con.Close();
            }
            return "Connection Successful";
        }
    }
}
