using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LCPW.PR
{
    public partial class FnF : System.Web.UI.Page
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
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            showReport();
        }

        private void showReport()
        {
            string EmpName = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                EmpName = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            string query = "SELECT EMP_NO,OLD_EMP_NO,EMP_FIRST_NAME,CONFIRMATION_DATE,APPOINTMENT_DATE,DESIGNATION,DEPARTMENT,STATUS,WORKLOCATION,CITY,REGIONS,EFFECTIVE_DATE,REASON,PAYMENT_TYPE,NIC_NO, RESIGN_REMARKS,PRINT_SEQ,COMPENSATION,AMOUNT FROM  " + Session["Schema_Name"] + "prv_resigndetails where EMP_NO IN ('"+EmpName+"') ";
            string Empty = "";
          

            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query, string.Join(" ", EmpName), Empty, Empty);
            ReportDataSource rds = new ReportDataSource("FnfData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./LCPW/PR/FnF.rdlc";
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("EmpName",EmpName),
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
            string query = "select distinct EMP_NO,EMP_FIRST_NAME from " + Session["Schema_Name"] + "prv_resigndetails";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox1.DataSource = ds.Tables[0];
            ListBox1.DataTextField = ds.Tables[0].Columns["EMP_FIRST_NAME"].ToString();
            ListBox1.DataValueField = ds.Tables[0].Columns["EMP_NO"].ToString();
            ListBox1.DataBind();
        }

        protected void PrintButton_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=Fnf.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}