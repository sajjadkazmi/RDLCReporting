using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.OleDb;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
namespace WebApplication2
{
    public class Connection
    {
        public string create_connection()
        {
            string ConnectionString = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 52.148.86.1)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl.hh5omeip5wlutgscvhiqhuuamc.ix.internal.cloudapp.net)));USER ID=rbarep;PASSWORD=rbarep;";
            return ConnectionString;
        }


        //public string provider = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 52.148.86.1)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl.hh5omeip5wlutgscvhiqhuuamc.ix.internal.cloudapp.net)));USER ID=rbavari;PASSWORD=rbavari110#;";


        //public void create_con()
        //{
        //    OracleConnection CN = new OracleConnection();
        //    provider = CN.ConnectionString;

        //    string exception;
        //    exception = "";


        //    try
        //    {
        //        if (CN.State == ConnectionState.Closed)
        //        {
        //            CN.Open();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex.Message;
        //    }

        //}
        //public void close_con()
        //{
        //    string exception;
        //    exception = "";

        //    OracleConnection CN = new OracleConnection();
        //    provider = CN.ConnectionString;
        //    try
        //    {
        //        if (CN.State == ConnectionState.Open)
        //        {
        //            CN.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex.Message;
        //    }

        //}



    }
}