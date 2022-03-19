using Microsoft.VisualStudio.TestTools.UnitTesting;
using schedulesUnitedHosted.Shared;
using System;

namespace schedulesUnitedHosted.UnitTests
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void TestGetUserInfo()
        {
            var testUsers = getTestUsers();
            User test = await Http.GetFromJsonAsync<String>($"User/{testUsers.get(4).username}");
            Assert.AreEqual(testUsers.get(4), test);
        }

        [TestMethod]
        public void TestGetUserID()
        {
            var testUsers = getTestUsers();
            getUserInfo();
        }

        [TestMethod]
        public void TestCreateUser()
        {

        }

        [TestMethod]
        public void TestCreateDuplicateUser()
        {

        }

        [TestMethod]
        public void TestValidateUserCorrect()
        {

        }

        [TestMethod]
        public void TestValidateUserIncorrect()
        {

        }

        [TestMethod]
        public void TestValidateUserNonexistant()
        {

        }

        private List<User> getTestUsers()
        {
            var testUsers = new List<User>();
            testUsers.add(new User("bob", "bob1", "bobpass"));
            testUsers.add(new User("bob", "bob1", "bobpss"));
            testUsers.add(new User("sarah", "sar", "test"));
            testUsers.add(new User(1, "test", "test", "test"))
        }
    }
}
