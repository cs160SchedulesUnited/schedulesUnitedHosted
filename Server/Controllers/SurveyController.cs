using schedulesUnitedHosted.Shared;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace schedulesUnitedHosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        // GET <SurveyController>/responses/{eventID}
        [HttpGet("/responses/{eventID:int}")]
        public List<Response> getAllResponses(int eventID)
        {
            List<Response> ret = new List<Response>();
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

                        ret.Add(new Response(accountID, eventID, availableDay, availableHour));
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/responses/{eventID}/{date}
        [HttpGet("/responses/{eventID:int}/{date}")]
        public List<Response> getDayResponses(int eventID, string date)
        {
            string day = DBCon.Clean(date);
            List<Response> ret = new List<Response>();
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT * FROM Availabilities WHERE eventID = {eventID} AND availableDay = '{day}'", con);
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        int accountID = Int32.Parse(reader["accountID"].ToString());
                        DateTime availableDay = DateTime.Parse(reader["availableDay"].ToString());
                        int availableHour = Int32.Parse(reader["availableHour"].ToString());

                        ret.Add(new Response(accountID, eventID, availableDay, availableHour));
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
            List<Survey> ret = new List<Survey>();
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

                        ret.Add(new Survey(id, name, start, end, host, responses));
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
                MySqlCommand getSurvey = new MySqlCommand($"SELECT eventID FROM CalendarEvents WHERE eventName = '{cleaned}' AND eventHost = {ownerID}", con);
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
            List<Response> ret = new List<Response>();
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT * FROM Availabilities WHERE eventID = {surveyID} AND accountID = {accountID}", con);
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        DateTime availableDay = DateTime.Parse(reader["availableDay"].ToString());
                        int availableHour = Int32.Parse(reader["availableHour"].ToString());

                        ret.Add(new Response(accountID, surveyID, availableDay, availableHour));
                        i++;
                    }
                con.Close();
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
                //throw new Exception($"INSERT INTO Availabilities VALUES ({create.AccId}, {create.EventId}, '{create.Availability.ToString("yyyy-MM-dd")}', {create.Hour})");
                MySqlCommand createSurvey = new MySqlCommand($"INSERT INTO Availabilities VALUES ({create.AccId}, {create.EventId}, '{create.Availability.ToString("yyyy-MM-dd")}', {create.Hour})", con);
                createSurvey.ExecuteNonQuery();
                con.Close();
            }
        }

        // POST <SurveyController>
        [HttpPost("delete")]
        public void deleteResponse([FromBody] Response delete)
        {
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                //Create survey
                //throw new Exception($"DELETE FROM Availabilities WHERE accountID = {delete.AccId} AND eventID = {delete.EventId} AND availableDay = '{delete.Availability.ToString("yyyy-MM-dd")}' AND availableHour = {delete.Hour}");
                MySqlCommand deleteSurvey = new MySqlCommand($"DELETE FROM Availabilities WHERE accountID = {delete.AccId} AND eventID = {delete.EventId} AND availableDay = '{delete.Availability.ToString("yyyy-MM-dd")}' AND availableHour = {delete.Hour}", con);
                deleteSurvey.ExecuteNonQuery();
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
                MySqlCommand createSurvey = new MySqlCommand($"INSERT INTO CalendarEvents VALUES (NULL, '{create.name}', '{create.start.ToString("yyyy-MM-dd")}', '{create.end.ToString("yyyy-MM-dd")}', {create.host})", con);
                createSurvey.ExecuteNonQuery();
                con.Close();
            }
            if (create.Responses.Count > 0)
            {
                int id = getSurveyID(create.name, create.host);
                for(int i = 0; i < create.Responses.Count; i++)
                {
                    Response temp = create.Responses[i];
                    temp.EventId = id;
                    createResponse(create.Responses[i]);
                }
            }
        }

        // POST <SurveyController>
        [HttpPost("edit")]
        public void EditSurvey([FromBody] UserSurvey combined)
        {
            User owner = combined.user;
            Survey edit = combined.survey;
            User cleaned = DBCon.Clean(owner);
            if (getOneSurvey(edit.id).host == cleaned.accountID)
            {
                string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                DBCon conGen = new DBCon(conString);
                MySqlConnection con = conGen.GetConnection();
                using (con)
                {
                    con.Open();
                    //Delete survey
                    MySqlCommand editSurvey = new MySqlCommand($"UPDATE CalendarEvents SET eventName = '{DBCon.Clean(edit.name)}', startDate = '{edit.start.ToString("yyyy-MM-dd")}', endDate = '{edit.end.ToString("yyyy-MM-dd")}' WHERE eventID = {edit.id}", con);
                    editSurvey.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        // POST <SurveyController>
        [HttpPost("delete/{id:int}")]
        public void DeleteSurvey(int id, [FromBody] User owner)
        {
            User cleaned = DBCon.Clean(owner);
            var survey = getOneSurvey(id);
            if(survey != null)
            {
                if(survey.host == cleaned.accountID)
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
                        deleteSurvey = new MySqlCommand($"DELETE FROM Availabilities WHERE eventID = {id}", con);
                        deleteSurvey.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    throw new Exception("User is not owner");
                }
            }
            else
            {
                throw new Exception("Survey doesn't exist");
            }
        }
    }
}
