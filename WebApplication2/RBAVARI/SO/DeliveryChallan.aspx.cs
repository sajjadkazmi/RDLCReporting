using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.RBAVARI.SO
{
    public partial class DeliveryChallan : System.Web.UI.Page
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

            string InvoiceNumber = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                InvoiceNumber = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }

            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", InvoiceNumber));

            ReportDataSource rds = new ReportDataSource("InvoiceData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/SO/DeliveryChallan.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("InvoiceNumber",InvoiceNumber),
                    //new ReportParameter ("CustName",CustName),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        private DataTable GetData(string InvoiceNumber)
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
                OracleDataAdapter da = new OracleDataAdapter("select * from rbavari.sov_invoice where TRXREF IN ('" + InvoiceNumber + "')", con);
                DataTable dt = new DataTable("DemoDt");

                //InvoiceData.DataTable1DataTable dtt = new InvoiceData.DataTable1DataTable();

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
                    OracleCommand com = new OracleCommand("select distinct abname, address_code from  " + Session["schema_name"] + "Gcv_AddressBook where user_name = '" + Session["u_id"] + "' order by AbName ", con);
                    OracleDataAdapter daa = new OracleDataAdapter(com);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox2.DataSource = dss.Tables[0];
                    ListBox2.DataTextField = dss.Tables[0].Columns["abname"].ToString();
                    ListBox2.DataValueField = dss.Tables[0].Columns["address_code"].ToString();
                    ListBox2.DataBind();

                }
                con.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Connection getCon = new Connection();
            string connectString = getCon.create_connection();
            //string schema_name = "rbavari.";
            string value2 = "";
            string CustName = "";

            foreach (int i in ListBox2.GetSelectedIndices())
            {
                value2 = value2 + "'" + ListBox2.Items[i].Value + "',";
                CustName = string.Join(" ", value2.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            try
            {

                OracleConnection con = new OracleConnection(connectString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    //listbox2
                    OracleCommand comm = new OracleCommand("select distinct TRXREF from " + Session["schema_name"] + "sov_invoice where CUSTOMER_CODE IN('" + CustName + "') ", con);

                    OracleDataAdapter da = new OracleDataAdapter(comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox1.DataSource = ds.Tables[0];
                    ListBox1.DataTextField = ds.Tables[0].Columns["TRXREF"].ToString();
                    ListBox1.DataValueField = ds.Tables[0].Columns["TRXREF"].ToString();
                    ListBox1.DataBind();


                }
                con.Close();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void PrintButton_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=DeliveryChallan.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}