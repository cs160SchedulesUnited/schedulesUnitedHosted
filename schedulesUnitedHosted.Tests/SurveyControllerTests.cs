using System;
using Xunit;
using schedulesUnitedHosted.Server.Controllers;
using schedulesUnitedHosted.Shared;
using System.Collections.Generic;

namespace schedulesUnitedHosted.Tests
{
    public class SurveyControllerTests
    {
        [Fact]
        public void getAllResponsesExists()
        {
            var controller = new SurveyController();
            var testResponses = getTestResponses();
            controller.createResponse(testResponses[0]);
            controller.createResponse(testResponses[1]);
            var test = controller.getAllResponses(1);
            controller.deleteResponse(testResponses[1]);
            controller.deleteResponse(testResponses[0]);
            Assert.Equal(testResponses[0].ToString(), test[0].ToString());
        }

        [Fact]
        public void getDayResponsesExists()
        {
            var controller = new SurveyController();
            var testResponses = getTestResponses();
            controller.createResponse(testResponses[0]);
            controller.createResponse(testResponses[1]);
            DateTime day = new DateTime(1999, 1, 1);
            var s = day.ToString("yyyy-MM-dd");
            var test = controller.getDayResponses(2, s);
            controller.deleteResponse(testResponses[1]);
            controller.deleteResponse(testResponses[0]);
            Assert.Equal(testResponses[1].ToString(), test[0].ToString());
        }

        [Fact]
        public void getAllResponsesNonexistent()
        {
            var controller = new SurveyController();
            var testResponses = getTestResponses();
            var test = controller.getAllResponses(int.MaxValue);
            Assert.Empty(test);
        }

        [Fact]
        public void getAllSurveyExists()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            testSurveys.RemoveAt(1);
            var test = controller.getAllSurvey(testSurveys[0].host);
            Assert.Equal(testSurveys.ToString(), test.ToString());
        }

        [Fact]
        public void getAllSurveyNonexistent()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getAllSurvey(int.MaxValue);
            Assert.Empty(test);
        }

        [Fact]
        public void getOneSurveyExists()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getOneSurvey(testSurveys[0].id);
            Assert.Equal(testSurveys[0].ToString(), test.ToString());
        }

        [Fact]
        public void getOneSurveyNonexistent()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getOneSurvey(int.MaxValue);
            Assert.Null(test);
        }

        [Fact]
        public void getSurveyIDExists()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getSurveyID(testSurveys[0].name, testSurveys[0].host);
            Assert.Equal(testSurveys[0].id, test);
        }

        [Fact]
        public void getSurveyIDNonexistent()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getSurveyID(testSurveys[0].name, int.MaxValue);
            Assert.Equal(0, test);
        }

        [Fact]
        public void getUsersResponsesExists()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getUsersResponses(testSurveys[0].id, testSurveys[0].Responses[0].AccId);
            Assert.Equal(testSurveys[0].Responses.ToString(), test.ToString());
        }

        [Fact]
        public void getUsersResponsesNonexistent()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getUsersResponses(int.MaxValue, int.MaxValue);
            Assert.Empty(test);
        }

        [Fact]
        public void createResponse()
        {
            var controller = new SurveyController();
            var testResponses = getTestResponses();
            controller.deleteResponse(testResponses[1]);
            controller.createResponse(testResponses[1]);
            var test = controller.getUsersResponses(testResponses[1].EventId, testResponses[1].AccId);
            controller.deleteResponse(testResponses[1]);
            Assert.Equal(testResponses[1].ToString(), test[0].ToString());
        }

        [Fact]
        public void createSurvey()
        {
            var controller = new SurveyController();
            var testUsers = getTestUsers();
            var testSurveys = getTestSurveys();
            testSurveys[1].id = controller.getSurveyID(testSurveys[1].name, testSurveys[1].host);
            try
            {
                controller.DeleteSurvey(testSurveys[1].id, getTestUsers()[0]);
            }catch(Exception e)
            {
                //Ignore it
            }
            controller.CreateSurvey(testSurveys[1]);
            var id = controller.getSurveyID(testSurveys[1].name, testSurveys[1].host);
            testSurveys[1].id = id;
            var test = controller.getOneSurvey(id);
            controller.DeleteSurvey(testSurveys[1].id, testUsers[0]);
            Assert.Equal(testSurveys[1].ToString(), test.ToString());
        }

        [Fact]
        public void editSurveyExists()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var testUsers = getTestUsers();
            controller.CreateSurvey(testSurveys[1]);
            var id = controller.getSurveyID(testSurveys[1].name, testSurveys[1].host);
            testSurveys[1].id = id;
            testSurveys[2].id = id;
            controller.EditSurvey(new UserSurvey(testSurveys[2], testUsers[0]));
            var test = controller.getOneSurvey(id);
            controller.DeleteSurvey(testSurveys[2].id, getTestUsers()[0]);
            Assert.Equal(testSurveys[2].ToString(), test.ToString());
        }

        [Fact]
        public void deleteSurveyExists()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            controller.CreateSurvey(testSurveys[1]);
            var id = controller.getSurveyID(testSurveys[1].name, testSurveys[1].host);
            testSurveys[1].id = id;
            controller.DeleteSurvey(id, getTestUsers()[0]);
            id = controller.getSurveyID(testSurveys[1].name, testSurveys[1].host);
            Assert.Equal(0, id);
        }

        [Fact]
        public void getInvitedUsersExists()
        {
            var controller = new SurveyController();
            var ids = controller.getInvitedUsers(1);
            Assert.Equal(2, ids[0]);
        }

        [Fact]
        public void getInvitedUsersNonexistent()
        {
            var controller = new SurveyController();
            var ids = controller.getInvitedUsers(-1);
            Assert.Empty(ids);
        }

        public List<Response> getTestResponses()
        {
            List<Response> testResponses = new List<Response>();
            testResponses.Add(new Response(1, 1, new DateTime(2022, 4, 1), 14));
            testResponses.Add(new Response(1, 2, new DateTime(1999, 1, 1), 1));
            return testResponses;
        }

        public List<Survey> getTestSurveys()
        {
            List<Survey> testSurveys = new List<Survey>();
            List<Response> temp = getTestResponses();
            temp.RemoveAt(1);
            testSurveys.Add(new Survey(1, "test", new DateTime(2022, 1, 1), new DateTime(2022, 5, 10), 1, temp));
            testSurveys.Add(new Survey("thi", new DateTime(2000, 2, 2), new DateTime(2000, 3, 3), 1, new List<Response>()));
            testSurveys.Add(new Survey(2, "test2", new DateTime(1999, 1, 1), new DateTime(2000, 1, 1), 1, new List<Response>()));
            return testSurveys;
        }

        public List<User> getTestUsers()
        {
            List<User> testUsers = new List<User>();
            // will always be in the database as a test user
            testUsers.Add(new User(1, "test", "test", "test"));
            // should be removed after each test
            testUsers.Add(new User("cre", "cre", "cre"));
            testUsers.Add(new User("cre", "cre", "bob"));
            testUsers.Add(new User("del", "del", "del"));
            // should never be added, to be used as a non-existent user
            testUsers.Add(new User("sarah", "sar", "s123"));
            return testUsers;
        }
    }
}
