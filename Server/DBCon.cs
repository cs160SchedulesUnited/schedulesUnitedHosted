using MySql.Data.MySqlClient;
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
    }
}
