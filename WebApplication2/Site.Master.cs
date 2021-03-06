﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.IO;

namespace WebApplication2
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["u_id"] == null)
            //{
            //    Response.Redirect("~/Action/Login.aspx");
            //}
            //else
            //{
            //    if (!this.IsPostBack)
            //    {
            //        //Label1.Text = Session["u_id"].ToString();
            //        DataTable dt = this.GetMenuData();
            //        PopulateMenu(dt);
            //    }
            //}
            //GetCompanyDetail();
        }


        //public void GetCompanyDetail()
        //{
        //    Connection getCon = new Connection();
        //    string connectString = getCon.create_connection();
        //    string CompanyName = "";
        //    byte[] bytes;


        //    try
        //    {
        //        OracleConnection con = new OracleConnection(connectString);
        //        con.Open();
        //        OracleCommand cmd = new OracleCommand("select rems,co_logo from " +Session["Schema_Name"] + " gccompany where ROWNUM <= 1 ", con);

        //        OracleDataReader dr = cmd.ExecuteReader();
        //        int FieldCount = dr.FieldCount;
        //        if (dr.HasRows)
        //        {
        //            while (dr.Read())
        //            {
        //                CompanyName = (string)dr["REMS"];
        //                bytes = (byte[])dr["CO_LOGO"];

        //                //byte[] imgBinary = File.filenam .ReadAllBytes("image.png");

        //                string imgString = Convert.ToBase64String(bytes);

        //                //Creating Session
        //                Session["REMS"] = CompanyName;
        //                Session["CO_LOGO"] = imgString;


        //            }
        //            con.Close();
        //        }
        //        else
        //        {
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        //HttpContext.Current.Response.Redirect("~/Action/login.aspx");

        //        throw;
        //    }
        //}
        //private void PopulateMenu(DataTable dt)
        //{
        //    string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
        //    DataView view = new DataView(dt);
        //    view.RowFilter = "mnu_id like '%MAIN%'";
        //    foreach (DataRowView row in view)
        //    {
        //        MenuItem menuItem = new MenuItem
        //        {
        //            Value = row["sub_mnu_id"].ToString(),
        //            Text = row["rems"].ToString(),
        //            NavigateUrl = row["report_url"].ToString(),
        //            Selected = row["report_url"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
        //        };
        //        Menu1.Items.Add(menuItem);
        //        AddChildItems(dt, menuItem);
        //    }
        //}


        //private void AddChildItems(DataTable table, MenuItem menuItem)
        //{
        //    DataView viewItem = new DataView(table);
        //    //viewItem.RowFilter = "mnu_id=" + menuItem.Value;
        //    viewItem.RowFilter = "mnu_id = '" + menuItem.Value + "' ";

        //    foreach (DataRowView childView in viewItem)
        //    {
        //        MenuItem childmenuItem = new MenuItem
        //        {
        //            Value = childView["sub_mnu_id"].ToString(),
        //            Text = childView["rems"].ToString(),
        //            NavigateUrl = childView["report_url"].ToString(),
        //        };
        //        menuItem.ChildItems.Add(childmenuItem);
        //        AddChildItems(table, childmenuItem);
        //    }
        //}

        //private DataTable GetMenuData()
        //{
        //    object userNameSession = Session["u_id"];
        //    Connection getCon = new Connection();
        //    string connectString = getCon.create_connection();

        //    try
        //    {
        //        OracleConnection con = new OracleConnection(connectString);
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        OracleCommand comm = new OracleCommand("select mnu_id, sub_mnu_id, rems, report_url from " + Session["Schema_Name"] + " ReportMenu where auth_level like '%" + Session["auth_level"] + "%'  ", con);

        //        OracleDataAdapter da = new OracleDataAdapter(comm);

        //        DataTable dt = new DataTable();
        //        da.Fill(dt); // fill dataset
        //        con.Close();
        //        return dt;
        //    }


        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
        

        protected void Delete(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();

            //HttpCookie cookie = Request.Cookies["UserDetails"];
            //cookie.Expires = DateTime.Now.AddDays(-1d);
            //Response.Cookies.Add(cookie);
            //Response.Cookies["Email_id"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("~/Action/Login.aspx");
        }
    }

}