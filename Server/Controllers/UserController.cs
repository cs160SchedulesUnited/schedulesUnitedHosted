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
        
        // TODO: Test this API
        // GET <UserController>/{#}
        [HttpGet("{un}")]
        public User getUserInfo(string un)
        {
            string cleaned = DBCon.Clean(un);

            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            User ret = null;
            using (con)
            {
                con.Open();
                MySqlCommand test = new MySqlCommand($"SELECT * FROM Accounts WHERE username = {un}");
                using (var reader = test.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["name"].ToString(), reader["username"].ToString(), reader["password"].ToString());
                    }
                con.Close();
            }
            return ret;
        }

        // TODO: Test this API
        // POST <UserController>/
        [HttpPost]
        public void Post([FromBody] User person)
        {
            User cleaned = DBCon.Clean(person);
            User ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                int id = person.accountID;
                string name = person.name;
                string username = person.username;
                string password = person.password;
                //TODO: write the SQL query that will insert the user into the DB
                MySqlCommand check = new MySqlCommand($"SELECT * FROM Accounts WHERE username = {username}");
                using (var reader = check.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["name"].ToString(), reader["username"].ToString(), reader["password"].ToString());
                    }
                if(ret.Equals(null))
                {
                    //Create user
                    MySqlCommand createUser = new MySqlCommand($"INSERT INTO Accounts VALUES (NULL, {name}, {username}, {password})");
                    createUser.ExecuteNonQuery();
                }
                else
                {
                    //Do nothing, return error, user already exists
                    throw new Exception("User already exists");
                }
                con.Close();
            }
        }
    }
}
