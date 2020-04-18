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
            string schema_name = "rbavari.";

            try
            {
                
                OracleConnection con = new OracleConnection(connectString);
                con.Open();
                OracleCommand cmd = new OracleCommand("select user_name, auth_level, PASS_CODE from "+schema_name+"gcpasscode where pass_code = '" + txtpasscode.Text + "' ", con);


                OracleDataReader dr = cmd.ExecuteReader();
                int FieldCount = dr.FieldCount;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        usr_name = (string)dr["user_name"];
                        string auth_level = (string)dr["auth_level"];
                        string PASS_CODE = (string)dr["PASS_CODE"];

                        //Creating Session

                        Session["u_id"] = usr_name;

                        HttpCookie cookie = new HttpCookie("UserDetails");
                        cookie["Email_id"] = Server.UrlEncode(usr_name);
                        cookie["AuthLevel"] = Server.UrlEncode(auth_level);

                        // Cookie will be persisted for 30 days
                        cookie.Expires = DateTime.Now.AddDays(30);
                        // Add the cookie to the client machine
                        Response.Cookies.Add(cookie);

                        Response.Redirect("~/Dashboard.aspx");
                    }
                }
                else
                {
                    lblMessage.Text = "Enter Corrrect passcode";
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