using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    public class GlobalReport
    {
        public string create_connection()
        {
            string ConnectionString = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 52.148.86.1)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl.hh5omeip5wlutgscvhiqhuuamc.ix.internal.cloudapp.net)));USER ID=rbarep;PASSWORD=rbarep;";
            return ConnectionString;
        }

        public DataTable GetData(string query,string param1, string param2, string param3)
        {
            //Connection getCon = new Connection();
            string connectString = create_connection();
            try
            {
                //string schema_name = "rbavari.";
                OracleConnection con = new OracleConnection(connectString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OracleDataAdapter da = new OracleDataAdapter(""+query+"", con);
                DataTable dt = new DataTable("DemoDt");
                da.Fill(dt);
                return dt;

                //con.Close();

            }
            catch (OracleException ex)
            {
                return null;
            }
        }

        public DataSet Listbox(string query)
        {
            Connection getCon = new Connection();
            string connectString = getCon.create_connection();
            try
            {
                //string schema_name = "rbavari.";
                OracleConnection con = new OracleConnection(connectString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OracleDataAdapter da = new OracleDataAdapter("" + query + "", con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;

                //con.Close();

            }
            catch (OracleException ex)
            {
                return null;
            }

        }

    }
}