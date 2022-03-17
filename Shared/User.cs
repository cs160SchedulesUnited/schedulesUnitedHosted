using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class User
    {
        public int accountID { get; set; }

        public string name { get; set; }

        public string username { get; set; }

        public string password { get; set; }
        public User(int accountID, string name, string username, string password)
        {
            this.accountID = accountID;
            this.name = name;
            this.username = username;
            this.password = password;
        }
    }
}
