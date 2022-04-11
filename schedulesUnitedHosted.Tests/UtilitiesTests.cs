using System;
using Xunit;
using schedulesUnitedHosted.Shared;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace schedulesUnitedHosted.Tests
{
    public class UtilitiesTests
    {
        [Fact]
        public void hashingTest()
        {
            Utilities util = new Utilities();
            String password = "test";
            int l = util.saltLength(password);
            String hash = util.hashPass(password);
            //byte[] salt = Encoding.ASCII.GetBytes(hash.Substring(0, 8));
            //String verify = util.hashPass(password, salt);
            Assert.True(util.verifyPass(password, hash));
        }
    }
}
