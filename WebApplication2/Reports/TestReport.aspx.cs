﻿using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Reports
{
    public partial class TestReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["u_id"] == null)
            {
                Response.Redirect("~/Action/Login.aspx");
            }

            if (!Page.IsPostBack)
            {

                BindListbox();
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            showReport();
        }

        private void showReport()
        {

            string ListBoxValues = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                ListBoxValues = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }

            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", ListBoxValues));
            //DataTable dt2 = GetData(string.Join(" ", ListBoxValues));

            ReportDataSource rds = new ReportDataSource("TestData", dt);
            //ReportDataSource rds2 = new ReportDataSource("TestData", dt2);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./Reports/Report1.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",ListBoxValues),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
        }

        private DataTable GetData(string Name)
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
                OracleDataAdapter da = new OracleDataAdapter("select a.cityName, a.custname, a.trxref, a.tpp_Date, a.value_date, a.DiscProfile, a.phaseref,a.itemname, a.packing_wt, a.order_qty, a.order_rate, a.grossamount, a.discamt, a.netamount from rbavari.sov_OrderMaster a where cust_code IN  ('" + Name + "')", con);
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

        private void BindListbox()
        {
            Connection getCon = new Connection();
            string connectString = getCon.create_connection();
            //string schema_name = "rbavari.";

            try
            {
               
                OracleConnection con = new OracleConnection(connectString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    OracleCommand comm = new OracleCommand("select distinct add_name, address_code  from " + Session["schema_name"] + "GcvAbList where UserName = 'sysmumtaz@gmail.com'  ", con);
                    OracleDataAdapter da = new OracleDataAdapter(comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox1.DataSource = ds.Tables[0];
                    ListBox1.DataTextField = ds.Tables[0].Columns["add_name"].ToString();
                    ListBox1.DataValueField = ds.Tables[0].Columns["address_code"].ToString();
                    ListBox1.DataBind();
                }
                con.Close();
              
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}