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
        public void GetPassCode(string pass)
        {
            //string pass = system.web.httpcontext.current.request["id"];
            Connection getCon = new Connection();
            string connectString = getCon.create_connection();
            string usr_name = "";
            string auth_level = "";
            string pass_code = "";
            string schema_name = "";

            try
            {
                OracleConnection con = new OracleConnection(connectString);
                con.Open();
                OracleCommand cmd = new OracleCommand("select user_name, auth_level, pass_code, schema_name from rbavari.gcpasscode where pass_code = '" + pass + "' ", con);

                OracleDataReader dr = cmd.ExecuteReader();
                int FieldCount = dr.FieldCount;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        usr_name = (string)dr["user_name"];
                        auth_level = (string)dr["auth_level"];
                        pass_code = (string)dr["pass_code"];
                        schema_name = (string)dr["schema_name"];
                        //Creating Session
                        System.Web.HttpContext.Current.Session["Pass_Code"] = pass_code;

                        System.Web.HttpContext.Current.Session["Auth_Level"] = auth_level;
                        System.Web.HttpContext.Current.Session["Schema_Name"] = schema_name;
                        System.Web.HttpContext.Current.Session["u_id"] = usr_name;

                    }
                    con.Close();
                    //Response.Redirect("~/Dashboard.aspx");
                }
                else
                {
                   HttpContext.Current.Response.Redirect("~/Action/login.aspx");
                    
                    //lblMessage.Text = "Please Provide a Corrrect Pass Code or Generate a New Pass Code from your Self Service";
                }
            }
            catch (Exception err)
            {
                   HttpContext.Current.Response.Redirect("~/Action/login.aspx");

                throw;
            }
        }

    }
}