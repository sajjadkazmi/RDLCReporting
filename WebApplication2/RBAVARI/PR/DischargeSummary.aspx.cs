using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.RBAVARI.PR
{
    public partial class DischargeSummary : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //string pass = Request["id"];
            //string passcode = string.Empty;
            //passcode = Convert.ToString(Session["Pass_Code"]);

            //if (Session["Pass_Code"] == null)
            //{
            //    GlobalReport GLRpt = new GlobalReport();
            //    GLRpt.GetPassCode(pass);
            //}
            //else if (Session["Pass_Code"] != null)
            //{
            //    GlobalReport GLRpt = new GlobalReport();
            //    GLRpt.GetPassCode(passcode);
            //}

            if (!Page.IsPostBack)
            {

                //BindListbox1();
                BindListbox2();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            showReport();
        }

        private void showReport()
        {
            //string ListBoxValues = "";
            //string value = "";
            //foreach (int i in ListBox1.GetSelectedIndices())
            //{
            //    value = value + "'" + ListBox1.Items[i].Value + "',";
            //    ListBoxValues = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            //}
            string FromDate = ListBox2.SelectedValue.ToString().Substring(0, 9);
            string ToDate = ListBox3.SelectedValue.ToString().Substring(0, 9);
            string query = "EMPLOYEMENT_STATUS";
            string Empty = "";
            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query, FromDate, ToDate, Empty);
            ReportDataSource rds = new ReportDataSource("AppSummaryData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/AppointSummary.rdlc";
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    //new ReportParameter ("Name",ListBoxValues),
                    new ReportParameter ("FromDate",FromDate),
                    new ReportParameter ("ToDate",ToDate),
                    new ReportParameter("CompanyName", Session["REMS"].ToString(),false),
                    new ReportParameter("CompanyLogo", Session["CO_LOGO"].ToString(),false),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        //private void BindListbox1()
        //{
        //    string query = "select distinct department from rbavari.prv_employeesalarysiml";
        //    GlobalReport GLRpt = new GlobalReport();
        //    DataSet ds = GLRpt.Listbox(query);

        //    ListBox1.DataSource = ds.Tables[0];
        //    ListBox1.DataTextField = ds.Tables[0].Columns["department"].ToString();
        //    ListBox1.DataValueField = ds.Tables[0].Columns["department"].ToString();
        //    ListBox1.DataBind();
        //}
        private void BindListbox2()
        {

            string query = "select distinct Appointment_date from rbavari.prv_employeesalarysiml order by Appointment_date DESC";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox2.DataSource = ds.Tables[0];
            ListBox2.DataTextField = ds.Tables[0].Columns["Appointment_date"].ToString();
            ListBox2.DataValueField = ds.Tables[0].Columns["Appointment_date"].ToString();
            ListBox2.DataTextFormatString = "{0:dd-MMM-yyyy}";
            ListBox2.DataBind();

            ListBox3.DataSource = ds.Tables[0];
            ListBox3.DataTextField = ds.Tables[0].Columns["Appointment_date"].ToString();
            ListBox3.DataValueField = ds.Tables[0].Columns["Appointment_date"].ToString();
            ListBox3.DataTextFormatString = "{0:dd-MMM-yyyy}";
            ListBox3.DataBind();
        }
        protected void PrintButton_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=AppointSummary.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}