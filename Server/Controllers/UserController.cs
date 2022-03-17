using schedulesUnitedHosted.Shared;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace schedulesUnitedHosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        
        // GET api/<UserController>/{#}
        [HttpGet("{id}")]
        public User getUserInfo(string id)
        {
            string cleaned = DBCon.Clean(id);

            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            User ret = null;
            using (con)
            {
                con.Open();
                //TODO: write the SQL query that will request the user with the matching userID
                MySqlCommand test = new MySqlCommand("");
                using (var reader = test.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(reader["accountID"].ToString(), reader["name"].ToString(), reader["username"].ToString(), reader["password"].ToString());
                    }
                con.Close();
            }
            return ret;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] User person)
        {
            User cleaned = DBCon.Clean(person);

            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                string id = person.accountID;
                string name = person.name;
                string username = person.username;
                string password = person.password;
                //TODO: write the SQL query that will insert the user into the DB
                MySqlCommand test = new MySqlCommand("");
                con.Close();
            }
        }
    }
}
