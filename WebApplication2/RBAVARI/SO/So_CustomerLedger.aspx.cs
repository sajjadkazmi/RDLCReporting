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
    public partial class So_CustomerLedger : System.Web.UI.Page
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
            string CustName = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                CustName = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            
            

            string date = datepicker.Text.ToString();
            var datetime = DateTime.Parse(date);
            var FromDate = datetime.ToString("dd-MMM-yyyy");

            string date1 = datepicker2.Text.ToString();
            var datetime1 = DateTime.Parse(date1);
            var ToDate = datetime1.ToString("dd-MMM-yyyy");


            //string ToDate = ListBox3.SelectedItem.ToString().Substring(0, 9);

            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", CustName), FromDate,ToDate);


            ReportDataSource rds = new ReportDataSource("CustomerLedgerData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/SO/CustomerLedger.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("CustName",CustName),
                    new ReportParameter ("FromDate",FromDate),
                    new ReportParameter ("ToDate",ToDate),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            ReportViewer1.KeepSessionAlive = false;
            ReportViewer1.AsyncRendering = false;
            //refresh
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        private DataTable GetData(string CustName,string FromDate,string ToDate)
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
                OracleDataAdapter da = new OracleDataAdapter("select grp_seq, grp_remarks, value_Date, cust_code,customer_name, discount_reference, item_name, debit_amount, credit_amount,  SUM(nvl(debit_amount, 0) - nvl(credit_amount, 0)) OVER(PARTITION BY cust_code order by value_date, grp_Seq, Item_name) RT from rbavari.Sov_Customerledger  where cust_code IN ('" + CustName + "') and value_date >= '" + FromDate+"' AND value_date < '"+ToDate+"'", con);
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
                    //listbox1
                    OracleCommand com = new OracleCommand("select distinct Customer_Name,cust_code from  " + Session["schema_name"] + "Sov_CustomerLedger order by Customer_Name ", con);
                    OracleDataAdapter daa = new OracleDataAdapter(com);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox1.DataSource = dss.Tables[0];
                    ListBox1.DataTextField = dss.Tables[0].Columns["Customer_Name"].ToString();
                    ListBox1.DataValueField = dss.Tables[0].Columns["cust_code"].ToString();
                    ListBox1.DataBind();

                    OracleCommand com2 = new OracleCommand("select distinct value_date from  " + Session["schema_name"] + "Sov_CustomerLedger order by value_date DESC ", con);
                    OracleDataAdapter da2 = new OracleDataAdapter(com2);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2); // fill dataset
                    //ListBox2.DataSource = ds2.Tables[0];
                    //ListBox2.DataTextField = ds2.Tables[0].Columns["value_date"].ToString();
                    //ListBox2.DataValueField = ds2.Tables[0].Columns["value_date"].ToString();
                    //ListBox2.DataTextFormatString = "{0:dd-MMM-yyyy}";
                    //ListBox2.DataBind();

                    //listbox3
                    //ListBox3.DataSource = ds2.Tables[0];
                    //ListBox3.DataTextField = ds2.Tables[0].Columns["value_date"].ToString();
                    //ListBox3.DataValueField = ds2.Tables[0].Columns["value_date"].ToString();
                    //ListBox3.DataTextFormatString = "{0:dd-MMM-yyyy}";
                    //ListBox3.DataBind();

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
            Response.AddHeader("Content-Disposition", "inline; filename=CustomerLedger.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}