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
    public partial class DeductionReport : System.Web.UI.Page
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
            string Deduction = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                Deduction = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
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

            string query = "select RegionName,TRXDATE,  department, emp_no, emp_first_name, f_name, designation, compensation,  case when nvl(allowance,0) > 0 then   allowance else Deduction  end Amount from " + Session["Schema_Name"] + " prv_monthlyvariables where compensation IN ('" + Deduction + "') AND TRXDATE >= '" + FromDate + "' AND TRXDATE <= '" + ToDate + "' ";
            string Empty = "";
            //Reset
            ReportViewer1.Reset();
            GlobalReport GLRpt = new GlobalReport();
            DataTable dt = GLRpt.GetData(query, Deduction, FromDate, ToDate);
            ReportDataSource rds = new ReportDataSource("AbsenteeData", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "./LCPW/PR/Deduction.rdlc";
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Deduction",Deduction),
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
            string query = "select distinct compensation from  " + Session["Schema_Name"] + " prv_monthlyvariables";
            GlobalReport GLRpt = new GlobalReport();
            DataSet ds = GLRpt.Listbox(query);

            ListBox1.DataSource = ds.Tables[0];
            ListBox1.DataTextField = ds.Tables[0].Columns["compensation"].ToString();
            ListBox1.DataValueField = ds.Tables[0].Columns["compensation"].ToString();
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