using MySql.Data.MySqlClient;
using schedulesUnitedHosted.Shared;
using System;
using System.Collections.Generic;

namespace schedulesUnitedHosted.Server
{
    public class DBCon
    {
        public string ConnectionString { get; set; }

        public DBCon(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        static public String Clean(String query)
        {
            String cleaned = query;
            // Prevents some forms of SQL injection, not complete but a good start
            cleaned = cleaned.Replace("'", "''");
            // Prevents string from overflowing text field of SQL DB
            if (cleaned.Length > 65535) cleaned = cleaned.Substring(0, 65535);
            
            return cleaned;
        }

        static public User Clean(User u)
        {
            u.accountID = Clean(u.accountID);
            u.name = Clean(u.name);
            u.username = Clean(u.username);
            //While we should check the password, I want to see if we can find a way to handle it such that we don't have to validate it for sql injection
            return u;
        }
    }
}
