using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebAppSimplifica2.Persistence;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Microsoft.Extensions.Options;
using System.Data;
using Raven.Abstractions.Data;

namespace WebAppSimplifica2.Connection
{

    public class Conn
    {
        //ConnectionStringSettingsCollection ConnectionStrings;
        private readonly ConnectionDbContext _context;
        private static string[] args;
        public ConnectionStringOptions options_xd;     

        //public Conn(IOptionsMonitor<ConnectionStringOptions> opcMonitor)
        //{
            //options_xd = opcMonitor.CurrentValue;
        //}

        //public IDbConnection CreateConnection() => new SqlConnection(options_xd.DevEventsCs);

        public static string getSql()
        {
            string cnnnn = "";
            string r = "";

            try
            {
                cnnnn = WebApplication.CreateBuilder().Configuration.GetConnectionString("DevEventsCsHom");

                //r = ConfigurationManager.ConnectionStrings["DevEventsCs"].ConnectionString;
            }
            catch (Exception)
            {

            }

            return cnnnn;
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(getSql());
            return conn;
        }

    }
}
