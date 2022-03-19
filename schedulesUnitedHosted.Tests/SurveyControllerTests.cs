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
            var test = controller.getAllResponses(1);
            Assert.Equal(testResponses, test);
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
            var test = controller.getAllSurvey(testSurveys[0].host);
            Assert.Equal(testSurveys, test);
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
            Assert.Equal(testSurveys[0], test);
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
            Assert.Equal(testSurveys[0].Responses, test);
        }

        [Fact]
        public void getUsersResponsesNonexistent()
        {
            var controller = new SurveyController();
            var testSurveys = getTestSurveys();
            var test = controller.getUsersResponses(testSurveys[0].id, testSurveys[0].Responses[0].AccId);
            Assert.Empty(test);
        }

        [Fact]
        public void createResponse()
        {

        }

        [Fact]
        public void createSurvey()
        {

        }

        [Fact]
        public void editSurveyExists()
        {

        }

        [Fact]
        public void editSurveyNonexistent()
        {

        }

        [Fact]
        public void deleteSurveyExists()
        {

        }

        public List<Response> getTestResponses()
        {
            List<Response> testResponses = new List<Response>();
            testResponses.Add(new Response(1, 1, new DateTime(2022, 4, 1), 14));
            return testResponses;
        }

        public List<Survey> getTestSurveys()
        {
            List<Survey> testSurveys = new List<Survey>();
            testSurveys.Add(new Survey(1, "test", new DateTime(2022, 1, 1), new DateTime(2022, 5, 10), 1, getTestResponses()));
            return testSurveys;
        }
    }
}
