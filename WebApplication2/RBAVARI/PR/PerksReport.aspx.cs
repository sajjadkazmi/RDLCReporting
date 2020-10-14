using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.RBAVARI.PR
{
    public partial class PerksReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pass = Request["id"];
            string passcode = string.Empty;
            passcode = Convert.ToString(Session["Pass_Code"]);

            if (Session["Pass_Code"] == null)
            {
                GlobalReport GLRpt = new GlobalReport();
                GLRpt.GetPassCode(pass);
            }
            else if (Session["Pass_Code"] != null)
            {
                GlobalReport GLRpt = new GlobalReport();
                GLRpt.GetPassCode(passcode);
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

            //string InvoiceNumber = "";
            //string value = "";
            //foreach (int i in ListBox1.GetSelectedIndices())
            //{
            //    value = value + "'" + ListBox1.Items[i].Value + "',";
            //    InvoiceNumber = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            //}

            string AllowCode = ListBox2.SelectedValue.ToString();
            string ProcessMonth = ListBox1.SelectedValue.ToString().Substring(0, 9);
            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(AllowCode, ProcessMonth);


            ReportDataSource rds = new ReportDataSource("PerksData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/pr/Perks.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",AllowCode),
                    new ReportParameter ("ProcessMonth",ProcessMonth),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            ReportViewer1.KeepSessionAlive = false;
            ReportViewer1.AsyncRendering = false;
            //refresh
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        private DataTable GetData(string AllowCode, string ProcessMonth)
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
                OracleDataAdapter da = new OracleDataAdapter("select emp_no, emp_first_name,  process_month, allow_code, rems, appoint_date, left_date, department_order, department_name,  designation, employee_status, month_days, days_worked, lwp, standard_gross, actual_gross, actual_deductions, actual_netpay, perks_amount from rbavari.prv_perks where allow_code='"+AllowCode+"' AND process_month = to_date('"+ProcessMonth+"','mm/dd/rrrr')", con);
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
                    //listbox2
                    OracleCommand com = new OracleCommand("select distinct * from  " + Session["schema_name"] + "prv_paycode", con);
                    OracleDataAdapter daa = new OracleDataAdapter(com);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox2.DataSource = dss.Tables[0];
                    ListBox2.DataTextField = dss.Tables[0].Columns["REMS"].ToString();
                    ListBox2.DataValueField = dss.Tables[0].Columns["allow_code"].ToString();
                    ListBox2.DataBind();

                    //listbox2
                    OracleCommand com2 = new OracleCommand("select distinct process_month from  " + Session["schema_name"] + "prv_perks order by process_month", con);
                    OracleDataAdapter da2 = new OracleDataAdapter(com2);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2); // fill dataset
                    ListBox1.DataSource = ds2.Tables[0];
                    ListBox1.DataTextField = ds2.Tables[0].Columns["process_month"].ToString();
                    ListBox1.DataValueField = ds2.Tables[0].Columns["process_month"].ToString();
                    ListBox1.DataTextFormatString = "{0:dd-MMM-yyyy}";
                    ListBox1.DataBind();

                }
                con.Close();

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        protected void PrintButton_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=Invoice.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}