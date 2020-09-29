using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.RBAVARI.GL
{
    public partial class PettyCash : System.Web.UI.Page
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

            string tpp_code = ListBox1.SelectedValue.ToString();
            string referenceNO = ListBox2.SelectedValue.ToString();
            string test = "";
            string query = "Select TPP_CODE,REF1,VALUE_DATE,HDR_REMARKS,BANK_NAME,ACCOUNT_CENTER,ACCOUNT_NAME,DTL_REMARKS,DR_AMT from rbavari.glv_pettycash where tpp_code = '"+tpp_code+"' AND REF1 = '"+referenceNO+"'";
            //Reset
            ReportViewer1.Reset();
            //datasource
            GlobalReport GLReports = new GlobalReport();
            DataTable dt =GLReports.GetData(query,tpp_code,referenceNO,test);
            ReportDataSource rds = new ReportDataSource("PettyCashData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/GL/PettyCash.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("TppCode",tpp_code),
                    new ReportParameter ("RefrenceNO",referenceNO),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            ReportViewer1.KeepSessionAlive = false;
            ReportViewer1.AsyncRendering = false;
            //refresh
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        private void BindListbox()
        {
            string query = "select tpp_rems,tpp_code from rbavari.gcv_tppcode where auth_level like '%" + Session["Auth_Level"] + "%' and mdl_cd = 'GL' ";
            string query2 = "select distinct REF1 from rbavari.glv_pettycash";
            GlobalReport GLReports = new GlobalReport();
            DataSet ds = GLReports.Listbox(query);

                    ListBox1.DataSource = ds.Tables[0];
                    ListBox1.DataTextField = ds.Tables[0].Columns["tpp_rems"].ToString();
                    ListBox1.DataValueField = ds.Tables[0].Columns["tpp_code"].ToString();
                    ListBox1.DataBind();
            DataSet ds2 = GLReports.Listbox(query2);

            ListBox2.DataSource = ds2.Tables[0];
            ListBox2.DataTextField = ds2.Tables[0].Columns["REF1"].ToString();
            ListBox2.DataValueField = ds2.Tables[0].Columns["REF1"].ToString();
            ListBox2.DataBind();

        }

        protected void PrintButton_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=PettyCash.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}