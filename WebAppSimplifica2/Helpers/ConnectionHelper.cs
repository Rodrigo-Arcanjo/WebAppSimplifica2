using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using WebAppSimplifica2.Connection;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace WebAppSimplifica2.Helpers
{
    public class ConnectionHelper
    {

        private SqlConnection sqlConn;

        public static string ExecQueryToJson(string sql, List<SqlParameter> listOfParams)
        {
            using (var con = Conn.GetConnection())
            {
                SqlDataReader dr = null;
                var cmd  =  new SqlCommand(sql, con);
                string json;

                try
                {
                    listOfParams.ForEach(item =>
                    {
                        cmd.Parameters.Add(item);
                    });

                    con.Open();

                    dr = cmd.ExecuteReader();

                    json = Util.ToJson(dr);
                }
                catch (Exception ex)
                {
                    throw new Exception("Houve um erro na Consulta: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
                return json;
            }
        }

        public static string ExecProcToJson(string sql, List<SqlParameter> listOfParams)
        {
            //var con = Conn.GetConnection()
            using (var con = Conn.GetConnection())
            {
                SqlDataReader dr = null;
                var cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string json;

                try
                {
                    listOfParams.ForEach((item) =>
                    {
                        cmd.Parameters.Add(item);
                    });

                    con.Open();

                    dr = cmd.ExecuteReader();

                    json = Util.ToJson(dr);
                   
                }
                catch(Exception ex) 
                {
                    throw new Exception("Houve um erro na Consulta: " + ex.Message);
                    //System.Exception: 'Houve um erro na Consulta: The ConnectionString property has not been initialized.'
                }
                finally 
                { 
                    con.Close(); 
                }

                return json;
            }
        }

        public SqlConnection openConnection()
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DevEventsCsHom"].ConnectionString;
                SqlConnection sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                this.sqlConn = sqlConn;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível acessar a base da dados do sistema.");
            }

            return sqlConn;
        }

        public SqlConnection getConnectionXd()
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DevEventsCsHom"].ConnectionString;
                SqlConnection sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                this.sqlConn = sqlConn;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível acessar a base de dados.");
            }

            return sqlConn;
        }

        public void closeConnection()
        {

            if (this.sqlConn != null)
            {
                sqlConn.Close();
            }

        }


    }
}
