using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Action
{
    
    public partial class Login : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["u_id"] != null)
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Connection getCon = new Connection();
            string connectString = getCon.create_connection();
            string usr_name = "";
            string auth_level = "";
            string PASS_CODE = "";
            string schema_name = "";

            try
            {
                
                OracleConnection con = new OracleConnection(connectString);
                con.Open();
                OracleCommand cmd = new OracleCommand("select user_name, auth_level, pass_code, schema_name from rbavari.gcpasscode where pass_code = '" + txtpasscode.Text + "' ", con);


                OracleDataReader dr = cmd.ExecuteReader();
                int FieldCount = dr.FieldCount;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        usr_name = (string)dr["user_name"];
                        auth_level = (string)dr["auth_level"];
                        PASS_CODE = (string)dr["pass_code"];
                        schema_name = (string)dr["schema_name"];


                        //Creating Session

                        //Session["UserName"] = usr_name;
                        Session["Schema_Name"] = schema_name;
                        Session["u_id"] = usr_name;
                        
                    }
                        con.Close();
                        Response.Redirect("~/Dashboard.aspx",false);
                }
                else
                {
                    lblMessage.Text = "Please Provide a Corrrect Pass Code or Generate a New Pass Code from your Self Service";
                }
            }
            catch (Exception err)
            {
                lblMessage.Text = err.Message;
             
                //throw;
            }

        


        }
    }
}