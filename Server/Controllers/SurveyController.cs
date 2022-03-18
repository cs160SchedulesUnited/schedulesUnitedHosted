using schedulesUnitedHosted.Shared;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace schedulesUnitedHosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        //TODO: getAllSurvey(string person), getOneSurvey(string id), getUsersResps(string user, string id)
        //TODO: create helper method getAllResps(string id)

        //TODO: createOne(), editOne(string id, User person), deleteOne(string id, User person)

        // Helper method, to get all responses to the given survey
        public Response[] getAllResponses(int eventID)
        {
            Response[] ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT * FROM Availabilities WHERE eventID = {eventID}");
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        int accountID = Int32.Parse(reader["accountID"].ToString());
                        DateTime availableDay = DateTime.Parse(reader["availableDay"].ToString());
                        int availableHour = Int32.Parse(reader["availableHour"].ToString());

                        ret[i] = new Response(accountID, eventID, availableDay, availableHour);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/surveys/{username}
        [HttpGet("/surveys/{userID: int}")]
        public Survey[] getAllSurvey(int userID)
        {
            Survey[] ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getSurvey = new MySqlCommand($"SELECT * FROM CalendarEvents WHERE eventHost = {userID}");
                int i = 0;
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        int id = Int32.Parse(reader["eventID"].ToString());
                        string name = reader["eventName"].ToString();
                        DateTime start = DateTime.Parse(reader["startDate"].ToString());
                        DateTime end = DateTime.Parse(reader["endDate"].ToString());
                        int host = Int32.Parse(reader["eventHost"].ToString());
                        Response[] responses = getAllResponses(Int32.Parse(reader["eventID"].ToString()));

                        ret[i] = new Survey(id, name, start, end, host, responses);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/survey/{surveyID}
        [HttpGet("survey/{surveyID: int}")]
        public Survey getOneSurvey(int surveyID)
        {
            int cleaned = surveyID;
            Survey ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getSurvey = new MySqlCommand($"SELECT * FROM CalendarEvents WHERE eventID = {cleaned}");
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        int id = Int32.Parse(reader["eventID"].ToString());
                        string name = reader["eventName"].ToString();
                        DateTime start = DateTime.Parse(reader["startDate"].ToString());
                        DateTime end = DateTime.Parse(reader["endDate"].ToString());
                        int host = Int32.Parse(reader["eventHost"].ToString());
                        Response[] responses = getAllResponses(Int32.Parse(reader["eventID"].ToString()));

                        ret = new Survey(id, name, start, end, host, responses);
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/{surveyID}/{username}
        [HttpGet("{surveyID: int}/{accountID: int}")]
        public Response[] getUsersResponses(int surveyID, string accountID)
        {
            Response[] all = getAllResponses(surveyID);
            Response[] ret = new Response[all.Length];
            int c = 0;
            // iterate through all responses
            for(int i = 0; i < all.Length; i++)
            {
                // if the response is from the requested user it adds it to the list
                if(all[i].AccId == accountID)
                {
                    ret[c] = all[i];
                    c++;
                }
            }
            // make all a smaller array such that it will be full.
            all = new Response[c];
            for(int i = 0; i < c; i++)
            {
                all[i] = ret[i];
            }
            return all;
        }

        // POST <SurveyController>
        [HttpPost("create")]
        public void createSurvey([FromBody] Survey create)
        {
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                //Create survey
                MySqlCommand createSurvey = new MySqlCommand($"INSERT INTO CalendarEvents VALUES (NULL, '{create.name}', '{create.start}', '{create.end}', '{create.host}')");
                createSurvey.ExecuteNonQuery();
                con.Close();
            }
        }

        // POST <SurveyController>
        [HttpPost("edit")]
        public void editSurvey([FromBody] Survey edit, [FromBody] User owner)
        {
            deleteSurvey(edit.id, owner);
            createSurvey(edit);
        }

        // POST <SurveyController>
        [HttpPost("delete")]
        public void deleteSurvey([FromBody] int id, [FromBody] User owner)
        {
            User cleaned = DBCon.Clean(owner);
            if(getOneSurvey(id).host == cleaned.accountID)
            {
                string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                DBCon conGen = new DBCon(conString);
                MySqlConnection con = conGen.GetConnection();
                using (con)
                {
                    con.Open();
                    //Delete survey
                    MySqlCommand deleteSurvey = new MySqlCommand($"DELETE FROM CalendarEvents WHERE eventID = {id}");
                    deleteSurvey.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
