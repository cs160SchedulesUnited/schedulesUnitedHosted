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
        [Produces("application/json")]
        public String Get()
        {
            //TODO: Make this an env variable if possible, our connection string should not be publicly visible
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            MySqlConnection con = new MySqlConnection(conString);
            using (con)
            {
                con.Open();
                MySqlCommand test = new MySqlCommand("USE newschema;SELECT version();", con);
                try
                {
                    MySqlDataReader reader = test.ExecuteReader();
                    while(reader.Read())
                    {
                        Console.Write(reader["version()"]);
                    }
                }catch(Exception e)
                {
                    Console.Write(con.State + "\n");
                    Console.Write(e.Message + "\n");
                }
                
                con.Close();
            }
            return "Connection Successful";
        }

        [HttpGet("{username}")]
        [Produces("application/json")]
        public string getUserInfo(string username)
        {
            return username;
        }
    }
}
