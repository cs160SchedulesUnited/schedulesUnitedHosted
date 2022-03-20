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

        public UserController() { }

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
                MySqlCommand test = new MySqlCommand($"SELECT * FROM Accounts WHERE username = '{cleaned}'", con);
                using (var reader = test.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["personName"].ToString(), reader["username"].ToString(), reader["accountPassword"].ToString());
                    }
                con.Close();
            }
            return ret;
        }

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
                MySqlCommand test = new MySqlCommand($"SELECT accountID FROM Accounts WHERE username = '{cleaned}'", con);
                using (var reader = test.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = Int32.Parse(reader["accountID"].ToString());
                    }
                con.Close();
            }
            return ret;
        }

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
                MySqlCommand check = new MySqlCommand($"SELECT * FROM Accounts WHERE username = '{username}'", con);
                using (var reader = check.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["personName"].ToString(), reader["username"].ToString(), reader["accountPassword"].ToString());
                    }
                if (ret == null)
                {
                    //Create user
                    MySqlCommand createUser = new MySqlCommand($"INSERT INTO Accounts VALUES (NULL, '{name}', '{username}', '{password}')", con);
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

        // POST <UserController>/delete
        [HttpPost("/delete")]
        public void deleteUser([FromBody] User person)
        {
            User cleaned = DBCon.Clean(person);
            User ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                try
                {
                    Validate(person);
                    MySqlCommand delete = new MySqlCommand($"DELETE FROM Accounts WHERE username = '{person.username}'", con);
                    delete.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // POST <UserController>/validate
        [HttpPost("/validate")]
        public Boolean Validate([FromBody] User person)
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
                MySqlCommand check = new MySqlCommand($"SELECT * FROM Accounts WHERE username = '{username}'", con);
                using (var reader = check.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = new User(Int32.Parse(reader["accountID"].ToString()), reader["personName"].ToString(), reader["username"].ToString(), reader["accountPassword"].ToString());
                    }
                if (ret == null)
                {
                    // Credentials incorrect, throw an error
                    throw new Exception("Username and/or Password incorrect");
                    return false;
                }
                else if(ret.username.Equals(username) && ret.password.Equals(password))
                {
                    // No error means a correct validation
                    return true;
                }
                else
                {
                    // Credentials incorrect, throw an error
                    throw new Exception("Username and/or Password incorrect");
                    return false;
                }
                con.Close();
            }
        }
    }
}
