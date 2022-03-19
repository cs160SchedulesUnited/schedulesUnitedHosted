using schedulesUnitedHosted.Shared;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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
        public List<Response> getAllResponses(int eventID)
        {
            List<Response> ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT * FROM Availabilities WHERE eventID = {eventID}", con);
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
        [HttpGet("/surveys/{userID:int}")]
        public List<Survey> getAllSurvey(int userID)
        {
            List<Survey> ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getSurvey = new MySqlCommand($"SELECT * FROM CalendarEvents WHERE eventHost = {userID}", con);
                int i = 0;
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        int id = Int32.Parse(reader["eventID"].ToString());
                        string name = reader["eventName"].ToString();
                        DateTime start = DateTime.Parse(reader["startDate"].ToString());
                        DateTime end = DateTime.Parse(reader["endDate"].ToString());
                        int host = Int32.Parse(reader["eventHost"].ToString());
                        List<Response> responses = getAllResponses(Int32.Parse(reader["eventID"].ToString()));

                        ret[i] = new Survey(id, name, start, end, host, responses);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/survey/{surveyID}
        [HttpGet("survey/{surveyID:int}")]
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
                MySqlCommand getSurvey = new MySqlCommand($"SELECT * FROM CalendarEvents WHERE eventID = {cleaned}", con);
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        int id = Int32.Parse(reader["eventID"].ToString());
                        string name = reader["eventName"].ToString();
                        DateTime start = DateTime.Parse(reader["startDate"].ToString());
                        DateTime end = DateTime.Parse(reader["endDate"].ToString());
                        int host = Int32.Parse(reader["eventHost"].ToString());
                        List<Response> responses = getAllResponses(Int32.Parse(reader["eventID"].ToString()));

                        ret = new Survey(id, name, start, end, host, responses);
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/survey/{surveyName}/{ownerID}
        [HttpGet("survey/{surveyName}/{ownerID:int}")]
        public int getSurveyID(string surveyName, int ownerID)
        {
            string cleaned = DBCon.Clean(surveyName);
            int ret = 0;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getSurvey = new MySqlCommand($"SELECT eventID FROM CalendarEvents WHERE eventName = '{cleaned}' & eventHost = {ownerID}", con);
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = Int32.Parse(reader["eventID"].ToString());
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/{surveyID}/{username}
        [HttpGet("{surveyID:int}/{accountID:int}")]
        public List<Response> getUsersResponses(int surveyID, int accountID)
        {
            List<Response> ret = getAllResponses(surveyID);
            // iterate through all responses
            for(int i = 0; i < ret.Count; i++)
            {
                // if the response isn't from the requested user it removes it from the list
                if(ret[i].AccId == accountID)
                {
                    ret.RemoveAt(i);
                }
            }
            return ret;
        }

        // POST <SurveyController>
        [HttpPost("respond")]
        public void createResponse([FromBody] Response create)
        {
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                //Create survey
                MySqlCommand createSurvey = new MySqlCommand($"INSERT INTO Availabilities VALUES ({create.AccId}, {create.EventId}, '{create.Availability.ToString()}', {create.Hour})", con);
                createSurvey.ExecuteNonQuery();
                con.Close();
            }
        }

        // POST <SurveyController>
        [HttpPost("create")]
        public void CreateSurvey([FromBody] Survey create)
        {
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                //Create survey
                MySqlCommand createSurvey = new MySqlCommand($"INSERT INTO CalendarEvents VALUES (NULL, '{create.name}', '{create.start}', '{create.end}', '{create.host}')", con);
                createSurvey.ExecuteNonQuery();
                con.Close();
            }
        }

        // POST <SurveyController>
        [HttpPost("edit")]
        public void EditSurvey([FromBody] UserSurvey combined)
        {
            User owner = combined.user;
            Survey edit = combined.survey;
            DeleteSurvey(edit.id, owner);
            CreateSurvey(edit);
        }

        // POST <SurveyController>
        [HttpPost("delete/{id:int}")]
        public void DeleteSurvey(int id, [FromBody] User owner)
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
                    MySqlCommand deleteSurvey = new MySqlCommand($"DELETE FROM CalendarEvents WHERE eventID = {id}", con);
                    deleteSurvey.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
