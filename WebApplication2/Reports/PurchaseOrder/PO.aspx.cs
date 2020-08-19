using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Reports.PurchaseOrder
{
    public partial class PO : System.Web.UI.Page
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
            string approvalStatus = ListBox2.SelectedItem.ToString();
            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", ListBoxValues), approvalStatus);

            ReportDataSource rds = new ReportDataSource("POData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./Reports/PurchaseOrder/Report9.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("OrderNo",ListBoxValues),
                    new ReportParameter ("ApprovalStatus",approvalStatus),
                    new ReportParameter("USERID",Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
        }

        private DataTable GetData(string Name,string approvalStatus)
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
                OracleDataAdapter da = new OracleDataAdapter("select * from rbavari.pov_purchaseOrderMaster where TRXREF IN  ('" + Name + "') AND APPROVAL_STATUS ='"+approvalStatus+"' ", con);
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
                    OracleCommand com = new OracleCommand("select distinct TRXREF  from  rbavari.pov_purchaseOrderMaster ", con);
                    OracleDataAdapter da = new OracleDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox1.DataSource = ds.Tables[0];
                    ListBox1.DataTextField = ds.Tables[0].Columns["TRXREF"].ToString();
                    ListBox1.DataValueField = ds.Tables[0].Columns["TRXREF"].ToString();
                    ListBox1.DataBind();
                    //listbox2
                    OracleCommand comm = new OracleCommand("select  distinct  APPROVAL_STATUS  from  rbavari.pov_purchaseOrderMaster", con);
                    OracleDataAdapter daa = new OracleDataAdapter(comm);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox2.DataSource = dss.Tables[0];
                    ListBox2.DataTextField = dss.Tables[0].Columns["APPROVAL_STATUS"].ToString();
                    ListBox2.DataValueField = dss.Tables[0].Columns["APPROVAL_STATUS"].ToString();
                    ListBox2.DataBind();
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