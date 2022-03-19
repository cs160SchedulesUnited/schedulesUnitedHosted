using Microsoft.VisualStudio.TestTools.UnitTesting;
using schedulesUnitedHosted.Shared;
using schedulesUnitedHosted.Server.Controllers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace schedulesUnitedHosted.UnitTests
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void getUserInfoExists()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            User test = controller.getUserInfo(testUsers[0].username);
            Assert.AreEqual(test.toString(), testUsers[0].toString());
        }

        [TestMethod]
        public void getUserInfoNonexistent()
        {
        }

        [TestMethod]
        public void getUserIDExsists()
        {
        }

        [TestMethod]
        public void getUserIDNonexistent()
        {
        }

        [TestMethod]
        public void createUserNew()
        {
        }

        [TestMethod]
        public void createUserExists()
        {
        }

        [TestMethod]
        public void validateCorrect()
        {
        }

        [TestMethod]
        public void validateIncorrect()
        {
        }

        public List<User> getTestUsers()
        {
            List<User> testUsers = new List<User>();
            testUsers.Add(new User(1, "test", "test", "test"));
            testUsers.Add(new User("bob", "bob1", "bobp"));
            testUsers.Add(new User("bob", "bob1", "bobo"));
            testUsers.Add(new User("sarah", "sar", "s123"));
            return testUsers;
        }
    }
}
