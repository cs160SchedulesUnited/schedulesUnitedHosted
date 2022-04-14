using System;
using Xunit;
using schedulesUnitedHosted.Server.Controllers;
using schedulesUnitedHosted.Shared;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace schedulesUnitedHosted.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void DBConnectionTest()
        {
            var controller = new DBController();
            string test = controller.Get();
            Assert.Equal("Connection Successful", test);
        }

        [Fact]
        public void getUserInfoExists()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            User test = controller.getUserInfo(testUsers[0].username);
            Assert.Equal(test.toString(), testUsers[0].toString());
        }

        [Fact]
        public void getUserInfoNonexistent()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            User test = controller.getUserInfo(testUsers[4].username);
            Assert.Null(test);
        }

        [Fact]
        public void getUserInfoExistsID()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            User test = controller.getUserInfo(testUsers[0].accountID);
            Assert.Equal(test.toString(), testUsers[0].toString());
        }

        [Fact]
        public void getUserInfoNonexistentID()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            User test = controller.getUserInfo(testUsers[4].accountID);
            Assert.Null(test);
        }

        [Fact]
        public void getUserIDExsists()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            int test = controller.getUserID(testUsers[0].username);
            Assert.Equal(testUsers[0].accountID, test);
        }

        [Fact]
        public void getUserIDNonexistent()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            int test = controller.getUserID(testUsers[4].username);
            Assert.Equal(0, test);
        }

        [Fact]
        public void createUserNew()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                controller.deleteUser(testUsers[1]);
            }
            catch (Exception e)
            {
                //ignore it
            }
            controller.createUser(testUsers[1]);
            User test = controller.getUserInfo(testUsers[1].username);
            testUsers[1].accountID = controller.getUserID(testUsers[1].username);
            controller.deleteUser(testUsers[1]);
            Assert.Equal(testUsers[1].toString(), test.toString());
        }

        [Fact]
        public void createUserExists()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                controller.createUser(testUsers[0]);
                Assert.False(true);
            }
            catch (Exception e)
            {
                Assert.Equal("User already exists", e.Message);
            }
        }

        [Fact]
        public void deleteUserExists()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                controller.createUser(testUsers[3]);
            }catch(Exception e)
            {
                //ignore it
            }
            controller.deleteUser(testUsers[3]);
            int test = controller.getUserID(testUsers[3].username);
            Assert.Equal(0, test);
        }

        [Fact]
        public void deleteUserNonexistent()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                controller.deleteUser(testUsers[4]);
                Assert.False(true);
            }
            catch (Exception e)
            {
                Assert.Equal("Username and/or Password incorrect", e.Message);
            }
        }

        [Fact]
        public void validateCorrect()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                Assert.True(controller.Validate(testUsers[0]));
            } catch(Exception e)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void validateNonexistent()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                Boolean test = controller.Validate(testUsers[4]);
                Assert.False(test);
            }
            catch (Exception e)
            {
                Assert.Equal("Username and/or Password incorrect", e.Message);
            }
        }

        [Fact]
        public void validateIncorrect()
        {
            var controller = new UserController();
            var testUsers = getTestUsers();
            try
            {
                Boolean test = controller.Validate(testUsers[2]);
                Assert.False(test);
            }
            catch (Exception e)
            {
                Assert.Equal("Username and/or Password incorrect", e.Message);
            }
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
            testUsers.Add(new User(-1, "sarah", "sar", "s123"));
            return testUsers;
        }
    }
}
