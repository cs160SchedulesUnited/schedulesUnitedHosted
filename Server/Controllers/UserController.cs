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
        // TODO: Test this API
        // This API should not be used for validation
        // GET <UserController>/{username}
        [HttpGet("{username}")]
        public User getUserInfo(string username)
        {
            string cleaned = DBCon.Clean(username);

            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            User ret = null;
            using (con)
            {
                con.Open();
                MySqlCommand test = new MySqlCommand($"SELECT * FROM Accounts WHERE username = '{cleaned}'");
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
        // If this returns 0, the user was not found
        // GET <UserController>/id/{username}
        [HttpGet("id/{username}")]
        public int getUserID(string username)
        {
            string cleaned = DBCon.Clean(username);

            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            int ret = 0;
            using (con)
            {
                con.Open();
                MySqlCommand test = new MySqlCommand($"SELECT accountID FROM Accounts WHERE username = '{cleaned}'");
                using (var reader = test.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = Int32.Parse(reader["accountID"].ToString();
                    }
                con.Close();
            }
            return ret;
        }

        // TODO: Test this API
        // POST <UserController>
        [HttpPost]
        public void createUser([FromBody] User person)
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
                MySqlCommand check = new MySqlCommand($"SELECT * FROM Accounts WHERE username = '{username}'");
                using (var reader = check.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["name"].ToString(), reader["username"].ToString(), reader["password"].ToString());
                    }
                if (ret.Equals(null))
                {
                    //Create user
                    MySqlCommand createUser = new MySqlCommand($"INSERT INTO Accounts VALUES (NULL, '{name}', '{username}', '{password}')");
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

        // TODO: Test this API
        // POST <UserController>/validate
        [HttpPost("/validate")]
        public void Validate([FromBody] User person)
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
                MySqlCommand check = new MySqlCommand($"SELECT * FROM Accounts WHERE username = '{username}'");
                using (var reader = check.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["name"].ToString(), reader["username"].ToString(), reader["password"].ToString());
                    }
                if (ret.username.Equals(username) && ret.password.Equals(password))
                {
                    // No error means a correct validation
                }
                else
                {
                    // Credentials incorrect, throw an error
                    throw new Exception("Username and/or Password incorrect");
                }
                con.Close();
            }
        }
    }
}
