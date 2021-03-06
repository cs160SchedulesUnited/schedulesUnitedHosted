using schedulesUnitedHosted.Shared;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;

//TODO: Create APIs for the invite functions. CreateInvite, GetUsersInvites, GetSurveysInvites

namespace schedulesUnitedHosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        // GET <SurveyController>/responses/{eventID}
        /**
         * <param name="eventID">The ID of the event for which responses should be fetched, provide this in the URL</param>
         * <returns>A List of all responses to the survey</returns>
         */
        [HttpGet("responses/{eventID:int}")]
        [Produces("application/json")]
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
                        int availableHour = Int32.Parse(reader["availableTimes"].ToString());

                        ret.Add(new Response(accountID, eventID, availableDay, availableHour));
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/responses/{eventID}/{date}
        /**
         * <param name="eventID">The Id of the event for which responses are being requested, provide this in the URL</param>
         * <param name="date">The date as a string for which responses are being requested, provide this in the URL in the form: yyyy-MM-dd</param>
         * <returns>All responses to the survey on the given date</returns>
         */
        [HttpGet("responses/{eventID:int}/{date}")]
        [Produces("application/json")]
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
                        int availableHour = Int32.Parse(reader["availableTimes"].ToString());

                        ret.Add(new Response(accountID, eventID, availableDay, availableHour));
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/surveys/{username}
        /**
         * <param name="userID">The ID of the User whos Surveys are being requested</param>
         * <returns>All surveys owned by the User</returns>
         */
        [HttpGet("surveys/{userID:int}")]
        [Produces("application/json")]
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
                        int attendees = getNumAttedees(id);
                        Survey toAdd = new Survey(id, name, start, end, host, responses);
                        toAdd.numResponses = attendees;
                        //ret.Add(new Survey(id, name, start, end, host, responses));
                        ret.Add(toAdd);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/survey/{surveyID}
        /**
         * <param name="surveyID">The id of the survey being requested</param>
         * <returns>The survey object with the provided ID</returns>
         */
        [HttpGet("survey/{surveyID:int}")]
        [Produces("application/json")]
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
                        int attendees = getNumAttedees(surveyID);
                        ret = new Survey(id, name, start, end, host, responses);
                        ret.numResponses = attendees;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/survey/{surveyID}
        /**
         * <param name="surveyID">The id of the survey being requested</param>
         * <returns>The survey object with the provided ID</returns>
         */
        [HttpGet("attendees/{surveyID:int}")]
        [Produces("application/json")]
        public int getNumAttedees(int surveyID)
        {
            int cleaned = surveyID;
            Survey ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getSurvey = new MySqlCommand($"select count(distinct accountID) as c from Availabilities where eventID = {cleaned}", con);
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        int c = Int32.Parse(reader["c"].ToString());
                        return c;
                    }
                con.Close();
            }
            return 0;
        }

        // GET <SurveyController>/survey/{surveyID}
        /**
         * <param name="surveyID">The id of the survey being requested</param>
         * <returns>The survey object with the provided ID</returns>
         */
        [HttpGet("surveywh/{surveyID:int}")]
        [Produces("application/json")]
        public Survey getOneSurveyWithHost(int surveyID)
        {
            int cleaned = surveyID;
            Survey ret = null;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getSurvey = new MySqlCommand($"Select eventID, eventName, startDate, endDate, eventHost, username from CalendarEvents inner join Accounts on CalendarEvents.eventHost = Accounts.accountID where eventID = {cleaned}", con);
                using (var reader = getSurvey.ExecuteReader())
                    while (reader.Read())
                    {
                        int id = Int32.Parse(reader["eventID"].ToString());
                        string name = reader["eventName"].ToString();
                        DateTime start = DateTime.Parse(reader["startDate"].ToString());
                        DateTime end = DateTime.Parse(reader["endDate"].ToString());
                        int host = Int32.Parse(reader["eventHost"].ToString());
                        List<Response> responses = getAllResponses(Int32.Parse(reader["eventID"].ToString()));
                        string username = reader["username"].ToString();
                        int attendees = getNumAttedees(surveyID);
                        ret = new Survey(id, name, start, end, host, responses, username);
                        ret.numResponses = attendees;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/survey/{surveyName}/{ownerID}
        /**
         * <param name="ownerID">Id of the owner of the Survey</param>
         * <param name="surveyName">Name of the Survey being requested</param>
         * <returns>The ID of the requested Survey</returns>
         */
        [HttpGet("survey/{surveyName}/{ownerID:int}")]
        [Produces("application/json")]
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
        /**
         * <param name="surveyID">The ID of the survey whos responses are being requested</param>
         * <param name="accountID">The account id of the User whos responses are being requested</param>
         * <returns>All responses from the User on the Survey requested</returns>
         */
        [HttpGet("{surveyID:int}/{accountID:int}")]
        [Produces("application/json")]
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
                        int availableHour = Int32.Parse(reader["availableTimes"].ToString());

                        ret.Add(new Response(accountID, surveyID, availableDay, availableHour));
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/invite/{eventID}
        /**
         * <param name="eventID">The ID of the survey whos invites are being requested</param>
         * <returns>Ids of all users who have been invited to respond to the survey</returns>
         */
        [HttpGet("invite/{eventID:int}")]
        [Produces("application/json")]
        public List<int> getInvitedUsers(int eventID)
        {
            List<int> ret = new List<int>();
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT accountID FROM Invites WHERE eventID = {eventID}", con);
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        int userID = Int32.Parse(reader["accountID"].ToString());
                        ret.Add(userID);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/invite/nonresponded/{eventID}
        /**
         * <param name="eventID">The ID of the survey whos invites are being requested</param>
         * <returns>Ids of all users who have been invited to respond to the survey</returns>
         */
        [HttpGet("invite/nonresponded/{eventID:int}")]
        [Produces("application/json")]
        public List<int> getNonrespondingUsers(int eventID)
        {
            List<int> ret = new List<int>();
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT accountID FROM Invites WHERE eventID = {eventID} AND answered = 0", con);
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        int userID = Int32.Parse(reader["accountID"].ToString());
                        ret.Add(userID);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/invited/nonresponded/{userID}
        /**
         * <param name="userID">The ID of the user whos invites are being requested</param>
         * <returns>Ids of all surveys who the user has been invited to respond to, but has not yet responded to</returns>
         */
        [HttpGet("invited/nonresponded/{userID:int}")]
        [Produces("application/json")]
        public List<int> getInvitedSurveysUnanswered(int userID)
        {
            List<int> ret = new List<int>();
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT eventID FROM Invites WHERE accountID = {userID} AND answered = 0", con);
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        int eventID = Int32.Parse(reader["eventID"].ToString());
                        ret.Add(eventID);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // GET <SurveyController>/invited/nonresponded/{userID}
        /**
         * <param name="userID">The ID of the user whos invites are being requested</param>
         * <returns>Ids of all surveys who the user has been invited and responded to</returns>
         */
        [HttpGet("invited/responded/{userID:int}")]
        [Produces("application/json")]
        public List<int> getInvitedSurveysAnswered(int userID)
        {
            List<int> ret = new List<int>();
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();

            using (con)
            {
                con.Open();
                MySqlCommand getResponses = new MySqlCommand($"SELECT eventID FROM Invites WHERE accountID = {userID} AND answered = 1", con);
                int i = 0;
                using (var reader = getResponses.ExecuteReader())
                    while (reader.Read())
                    {
                        int eventID = Int32.Parse(reader["eventID"].ToString());
                        ret.Add(eventID);
                        i++;
                    }
                con.Close();
            }
            return ret;
        }

        // POST <SurveyController>/respond
        /**
         * <param name="create">The Response to be created, provided in the body of the POST, need all fields, if Hour is not wanted use 0 as a placeholder</param>
         */
        [HttpPost("respond")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public Boolean createResponse([FromBody] Response create)
        {
            Boolean fin = true;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                //Create survey
                //throw new Exception($"INSERT INTO Availabilities VALUES ({create.AccId}, {create.EventId}, '{create.Availability.ToString("yyyy-MM-dd")}', {create.Hour})");
                MySqlCommand createSurvey = new MySqlCommand($"INSERT INTO Availabilities VALUES (0, {create.AccId}, {create.EventId}, '{create.Availability.ToString("yyyy-MM-dd")}', {create.Hour})", con);
                createSurvey.ExecuteNonQuery();
                con.Close();
            }
            return fin;
        }

        // POST <SurveyController>/delete
        /**
         * this should only be available to the owner of the survey or the User who submitted the response
         * <param name="delete">The response to be deleted, provided in the body of the POST</param>
         */
        [HttpPost("delete")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public Boolean deleteResponse([FromBody] Response delete)
        {
            Boolean fin = true;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                //Create survey
                //throw new Exception($"DELETE FROM Availabilities WHERE accountID = {delete.AccId} AND eventID = {delete.EventId} AND availableDay = '{delete.Availability.ToString("yyyy-MM-dd")}' AND availableHour = {delete.Hour}");
                MySqlCommand deleteSurvey = new MySqlCommand($"DELETE FROM Availabilities WHERE accountID = {delete.AccId} AND eventID = {delete.EventId} AND availableDay = '{delete.Availability.ToString("yyyy-MM-dd")}' AND availableTimes = {delete.Hour}", con);
                deleteSurvey.ExecuteNonQuery();
                con.Close();
            }
            return fin;
        }

        // POST <SurveyController>/create
        /**
         * <param name="create">The survey that is to be created, provided in the body of the POST. The Survey ID field is not required, and should be requested once the survey is created. Additionally, if responses are present they will be added to the survey</param>
         */
        [HttpPost("create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public Boolean CreateSurvey([FromBody] Survey create)
        {
                Boolean fin = true;
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
                if (create.Responses != null && create.Responses.Count > 0)
                {
                    int id = getSurveyID(create.name, create.host);
                    for (int i = 0; i < create.Responses.Count; i++)
                    {
                        Response temp = create.Responses[i];
                        temp.EventId = id;
                        createResponse(create.Responses[i]);
                    }
                }
                return fin;
        }

        // POST <SurveyController>/edit
        /**
         * <param name="combined">The UserSurvey object that holds both the Survey and its Owning User, provided in the body of the POST. Only works if the provided user is the owner of the Survey</param>
         */
        [HttpPost("edit")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public Boolean EditSurvey([FromBody] UserSurvey combined)
        {
            Boolean fin = true;
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
                    //Edit survey
                    MySqlCommand editSurvey = new MySqlCommand($"UPDATE CalendarEvents SET eventName = '{DBCon.Clean(edit.name)}', startDate = '{edit.start.ToString("yyyy-MM-dd")}', endDate = '{edit.end.ToString("yyyy-MM-dd")}' WHERE eventID = {edit.id}", con);
                    editSurvey.ExecuteNonQuery();
                    con.Close();
                }
            }
            return fin;
        }

        // POST <SurveyController>/delete/{id}
        /**
         * <param name="combined">The UserSurvey object that holds both the Survey and its Owning User, provided in the body of the POST. Only works if the provided user is the owner of the Survey</param>
         * <exception cref="Exception">Throws an exception if the User is not the owner of the Survey, or if the survey doesn't exist</exception>
         */
        [HttpPost("delete/{id:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public Boolean DeleteSurvey(int id, [FromBody] User owner)
        {
            Boolean fin = true;
            User cleaned = DBCon.Clean(owner);
            var survey = getOneSurvey(id);
            if (survey != null)
            {
                if (survey.host == cleaned.accountID)
                {
                    string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                    DBCon conGen = new DBCon(conString);
                    MySqlConnection con = conGen.GetConnection();
                    using (con)
                    {
                        con.Open();
                        //Delete survey
                        MySqlCommand deleteSurvey = new MySqlCommand($"DELETE FROM Availabilities WHERE eventID = {id}", con);
                        deleteSurvey.ExecuteNonQuery();
                        deleteSurvey = new MySqlCommand($"DELETE FROM Invites WHERE eventID = {id}", con);
                        deleteSurvey.ExecuteNonQuery();
                        deleteSurvey = new MySqlCommand($"DELETE FROM CalendarEvents WHERE eventID = {id}", con);
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
            return fin;
        }

        // POST <SurveyController>/invite/{id}
        /**
         * <param name="survey">The The ID of the survey the user is being invited to, to be included in the body of the POST</param>
         * <param name="id">The id of the user being invited, to be included in the URL</param>
         */
        [HttpPost("invite/{id:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public Boolean inviteUser(int id, [FromBody] int survey)
        {
            Boolean fin = true;
            int ret = -1;
            string conString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            DBCon conGen = new DBCon(conString);
            MySqlConnection con = conGen.GetConnection();
            using (con)
            {
                con.Open();
                MySqlCommand check = new MySqlCommand($"SELECT * FROM Invites WHERE eventID = {survey} AND accountID = { id }", con);
                using (var reader = check.ExecuteReader())
                    while (reader.Read())
                    {
                        ret = Int32.Parse(reader["accountID"].ToString());
                    }
                if (ret == id) return false;
                MySqlCommand createInvite = new MySqlCommand($"INSERT INTO Invites (eventID, accountID) VALUES ({survey}, {id})", con);
                createInvite.ExecuteNonQuery();
                con.Close();
            }
            return fin;
        }
    }
}
