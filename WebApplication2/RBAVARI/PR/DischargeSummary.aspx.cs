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
            string pass = Request["id"];
            string passcode = string.Empty;
            passcode = Convert.ToString(Session["Pass_Code"]);

            if (Session["Pass_Code"] == null)
            {
                GlobalReport GLRpt = new GlobalReport();
                GLRpt.GetPassCode(pass);
            }
            else
            {
                GlobalReport GLRpt = new GlobalReport();
                GLRpt.GetPassCode(passcode);
            }

            if (!Page.IsPostBack)
            {

                BindListbox1();
                BindListbox2();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            showReport();
        }

        private void showReport()
        {
            string Department = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                Department = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            string FromDate = ListBox2.SelectedValue.ToString().Substring(0, 9);
            string ToDate = ListBox3.SelectedValue.ToString().Substring(0, 9);
            string query = "SELECT OLD_EMP_NO,EMPLOYEE_NAME,FATHER_SPOUSE_NAME,CNIC,APPOINTMENT_DATE,DESIGNATION,DEPARTMENT,LEFT_DATE,EMPLOYEMENT_STATUS,PROBATIONARY_DAYS FROM  " + Session["Schema_Name"] + " prv_EMPLOYEEMASTER where DEPARTMENT IN ('"+ Department + "') AND LEFT_DATE >= to_date('" + FromDate + "', 'mm/dd/rrrr') AND LEFT_DATE < to_date('" + ToDate + "', 'mm/dd/rrrr')  order by department_sequence";
            string Empty = "";
            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query, Department, FromDate, ToDate);
            ReportDataSource rds = new ReportDataSource("EmployeMasterData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/DischargeSummary.rdlc";
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

        private void BindListbox1()
        {
            string query = "select distinct DEPARTMENT from  " + Session["Schema_Name"] + " prv_EMPLOYEEMASTER";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox1.DataSource = ds.Tables[0];
            ListBox1.DataTextField = ds.Tables[0].Columns["DEPARTMENT"].ToString();
            ListBox1.DataValueField = ds.Tables[0].Columns["DEPARTMENT"].ToString();
            ListBox1.DataBind();
        }
        private void BindListbox2()
        {

            string query = "select distinct LEFT_DATE from " + Session["Schema_Name"] + " prv_EMPLOYEEMASTER order by LEFT_DATE DESC";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox2.DataSource = ds.Tables[0];
            ListBox2.DataTextField = ds.Tables[0].Columns["LEFT_DATE"].ToString();
            ListBox2.DataValueField = ds.Tables[0].Columns["LEFT_DATE"].ToString();
            ListBox2.DataTextFormatString = "{0:dd-MMM-yyyy}";
            ListBox2.DataBind();

            ListBox3.DataSource = ds.Tables[0];
            ListBox3.DataTextField = ds.Tables[0].Columns["LEFT_DATE"].ToString();
            ListBox3.DataValueField = ds.Tables[0].Columns["LEFT_DATE"].ToString();
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