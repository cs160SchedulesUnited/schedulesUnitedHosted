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
        /**
         * <param name="username">The username of the desired user information is to be included in the URL</param>
         * <returns>A User with accountID, personName, userName, and password</returns>
         */
        [HttpGet("{username}")]
        [Produces("application/json")]
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

        // This API should not be used for validation
        // GET <UserController>/{userID}
        /**
         * <param name="userID">The userID of the desired user information is to be included in the URL</param>
         * <returns>A User with accountID, personName, userName, and password</returns>
         */
        [HttpGet("info/{userID:int}")]
        [Produces("application/json")]
        public User getUserInfo(int userID)
        {
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            User ret = null;
            using (con)
            {
                con.Open();
                MySqlCommand test = new MySqlCommand($"SELECT * FROM Accounts WHERE accountID = '{userID}'", con);
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
        /**
         * <param name="username">The username of the desired userid is to be included in the URL</param>
         * <returns>User id, or 0 if user is not found</returns>
         */
        [HttpGet("id/{username}")]
        [Produces("application/json")]
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
        /**
         * <param name="person">Takes a User object as input from the body of the POST, userID is not needed in the provided User, once you create the user, you must call getUserId in order to get the correct UserId</param>
         */
        [HttpPost("/create")]
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
                    MySqlCommand createUser = new MySqlCommand($"INSERT INTO Accounts (personName, username, accountPassword) VALUES ('{name}', '{username}', '{password}')", con);
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
        /**
         * <param name="person">Requires User from body of POST, User object must exactly match the existing user or it throws an error</param>
         * <exception cref="Exception">If the user fails to validate, an exception is thrown</exception>
         */
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
        /**
         * <param name="person">Requires User from body of POST, User object must exactly match the existing user or it throws an error</param>
         * <exception cref="Exception">If the user fails to validate, an exception is thrown</exception>
         * <returns>Nothing with no errors if the user succesfully validates, might not be the best option</returns>
         */
        [HttpPost("/validate")]
        public Boolean Validate([FromBody] User person)
        {
            Utilities util = new Utilities();
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
                    //throw new Exception("Username and/or Password incorrect");
                    return false;
                }
                else if(ret.password.Equals(password))
                {
                    // No error means a correct validation
                    return true;
                }
                else
                {
                    // Credentials incorrect, throw an error
                    //throw new Exception("Username and/or Password incorrect");
                    return false;
                }
                con.Close();
            }
        }
    }
}
