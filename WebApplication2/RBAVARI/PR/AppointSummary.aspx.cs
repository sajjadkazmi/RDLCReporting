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
    public partial class AppointSummary : System.Web.UI.Page
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
            string ListBoxValues = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                ListBoxValues = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            string FromDate = ListBox2.SelectedValue.ToString().Substring(0, 9);
            string ToDate = ListBox3.SelectedValue.ToString().Substring(0, 9);
            string query = "select a.department, employee_name, old_emp_no, employee_no, cnic, a.Appointment_date, sum(a.Allowance) Cur_mnth_allow,   sum(a.Deduction) Cur_mnth_ded,  nvl(sum(a.Allowance), 0) - nvl(sum(a.deduction), 0) Cur_mnth_net from rbavari.prv_employeesalarysiml a where nvl(a.salary_based, 'N') = 'Y'   and nvl(a.Employer_Part, 'N') = 'N' and a.department IN  ('" + ListBoxValues + "') AND a.Appointment_date >= to_date('" + FromDate + "', 'mm/dd/rrrr') AND a.Appointment_date < to_date('" + ToDate + "', 'mm/dd/rrrr') group by a.department, employee_name, old_emp_no, employee_no, cnic, a.Appointment_date";
            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query,string.Join(" ", ListBoxValues), FromDate, ToDate);
            ReportDataSource rds = new ReportDataSource("AppSummaryData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/AppointSummary.rdlc";
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",ListBoxValues),
                    new ReportParameter ("FromDate",FromDate),
                    new ReportParameter ("ToDate",ToDate),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }
        
        private void BindListbox1()
        {
            string query = "select distinct department from rbavari.prv_employeesalarysiml";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox1.DataSource = ds.Tables[0];
            ListBox1.DataTextField = ds.Tables[0].Columns["department"].ToString();
            ListBox1.DataValueField = ds.Tables[0].Columns["department"].ToString();
            ListBox1.DataBind();
        }
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