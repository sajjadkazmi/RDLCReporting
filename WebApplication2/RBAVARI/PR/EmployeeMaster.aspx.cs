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
    public partial class EmployeeMaster : System.Web.UI.Page
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
                BindListbox1();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            showReport();
        }

        private void showReport()
        {
            string Status = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                Status = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            
            string query = "SELECT OLD_EMP_NO,EMPLOYEE_NAME,FATHER_SPOUSE_NAME,CNIC,APPOINTMENT_DATE,DESIGNATION,DEPARTMENT_CODE,DEPARTMENT_SEQUENCE,DEPARTMENT,LEFT_DATE,ACTIVE_CHECK,STANDARD_GROSS FROM  lcpw.prv_EMPLOYEEMASTER where ACTIVE_CHECK IN  ('" + Status + "') ";
            string Empty = "";
            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query,Status,Empty,Empty);
            ReportDataSource rds = new ReportDataSource("EmployeMasterData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/EmployeeMaster.rdlc";
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Status",Status),
                    //new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        private void BindListbox1()
        {
            string query = "select distinct active_check from   lcpw.prv_EMPLOYEEMASTER";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox1.DataSource = ds.Tables[0];
            ListBox1.DataTextField = ds.Tables[0].Columns["active_check"].ToString();
            ListBox1.DataValueField = ds.Tables[0].Columns["active_check"].ToString();
            ListBox1.DataBind();
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