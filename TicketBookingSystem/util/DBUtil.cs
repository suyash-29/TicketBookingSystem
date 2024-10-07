using System.Data.SqlClient;

using System;

namespace TicketBookingSystem.util
{
    public class DBUtil
    {
        private static readonly string connectionString = "Data Source=ssc29;Initial Catalog=TBS1;Integrated Security=True;MultipleActiveResultSets=True;";
        public static SqlConnection GetDBConn()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open(); 
            return conn;
        }
    }
}

//_connectionString = "Data Source=DESKTOP-12345;Initial Catalog=TicketBookingDB;Integrated Security=True;";
