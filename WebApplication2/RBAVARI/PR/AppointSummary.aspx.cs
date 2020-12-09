using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

            else
            {
                GlobalReport GLRpt = new GlobalReport();
                GLRpt.GetPassCode(passcode);
            }

            if (!Page.IsPostBack)
            {
                BindListbox1();
                //BindListbox2();
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

            string date = datepicker.Text.ToString();
            
            var datetime = DateTime.Parse(date);
            var FromDate = datetime.ToString("dd-MMM-yyyy");

            string date1 = datepicker2.Text.ToString();
            if (date1 == "")
            {
                date1 = DateTime.Now.ToString();
            }
            var datetime1 = DateTime.Parse(date1);
            var ToDate = datetime1.ToString("dd-MMM-yyyy");
            
            string query = "SELECT OLD_EMP_NO,EMPLOYEE_NAME,FATHER_SPOUSE_NAME,CNIC,APPOINTMENT_DATE,DESIGNATION,DEPARTMENT_CODE,DEPARTMENT_SEQUENCE,DEPARTMENT,LEFT_DATE,ACTIVE_CHECK,STANDARD_GROSS,BIRTH_DATE,SOCIAL_SECURITY,SKILL_LEVEL,REGION,PROBATIONARY_DAYS FROM  "+Session["Schema_Name"] + " prv_EMPLOYEEMASTER where DEPARTMENT IN('"+Department+"') AND Appointment_date >= '" + FromDate + "' AND Appointment_date < '" + ToDate + "'  order by REGION, department_sequence";

            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query,string.Join(" ", Department), FromDate, ToDate);
            ReportDataSource rds = new ReportDataSource("EmployeMasterData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/AppointSummary.rdlc";
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Department",Department),
                    new ReportParameter ("FromDate",FromDate),
                    new ReportParameter ("ToDate",ToDate),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false),
                    new ReportParameter("REMS", Session["REMS"].ToString(),false),
                    new ReportParameter("CO_LOGO", Session["CO_LOGO"].ToString(),false)
            };
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.SetParameters(rptParms);
            
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }
        
        private void BindListbox1()
        {
            string query = "select distinct department from " + Session["Schema_Name"] + "prv_EMPLOYEEMASTER";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox1.DataSource = ds.Tables[0];
            ListBox1.DataTextField = ds.Tables[0].Columns["department"].ToString();
            ListBox1.DataValueField = ds.Tables[0].Columns["department"].ToString();
            ListBox1.DataBind();
        }
        //private void BindListbox2()
        //{

        //    string query = "select distinct Appointment_date from " + Session["Schema_Name"] + "prv_EMPLOYEEMASTER order by Appointment_date DESC";
        //    GlobalReport GLRpt = new GlobalReport();
        //    DataSet ds = GLRpt.Listbox(query);

        //    ListBox2.DataSource = ds.Tables[0];
        //    ListBox2.DataTextField = ds.Tables[0].Columns["Appointment_date"].ToString();
        //    ListBox2.DataValueField = ds.Tables[0].Columns["Appointment_date"].ToString();
        //    ListBox2.DataTextFormatString = "{0:dd-MMM-yyyy}";
        //    ListBox2.DataBind();

        //    ListBox3.DataSource = ds.Tables[0];
        //    ListBox3.DataTextField = ds.Tables[0].Columns["Appointment_date"].ToString();
        //    ListBox3.DataValueField = ds.Tables[0].Columns["Appointment_date"].ToString();
        //    ListBox3.DataTextFormatString = "{0:dd-MMM-yyyy}";
        //    ListBox3.DataBind();
        //}

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