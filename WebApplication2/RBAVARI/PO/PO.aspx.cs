using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using WebApplication2.Reports.PurchaseOrder;

namespace WebApplication2.RBAVARI.PO
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
            string approvalStatus = ListBox3.SelectedItem.ToString();
            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", ListBoxValues), approvalStatus);

            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            //Update DataTable with barcode image
            foreach (POData.DataTable1Row row in dt.Rows)
            {
                //Set the value to encode
                //bcp.Code = row.TRXREF.ToString();
                QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(row.TRXREF.ToString(), QRCoder.QRCodeGenerator.ECCLevel.Q);
                QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);
                Bitmap bmp = qRCode.GetGraphic(5);

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Bmp);
                    row.BarCode = ms.ToArray();
                    //reportData.QRCode.AddQRCodeRow(qRCodeRow);
                }
            }
            ReportDataSource rds = new ReportDataSource("POData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PO/PO.rdlc";
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
            PrintButton.Visible = true;

        }

        private DataTable GetData(string Name, string approvalStatus)
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
                OracleDataAdapter da = new OracleDataAdapter("select * from rbavari.pov_purchaseOrderMaster where TRXREF IN  ('" + Name + "') AND APPROVAL_STATUS ='" + approvalStatus + "' ", con);
                DataTable dt = new DataTable("DemoDt");

                POData.DataTable1DataTable dtt = new POData.DataTable1DataTable();
                da.Fill(dtt);

                //da.Fill(dt);
                con.Close();
                return dtt;

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
                    //listbox3
                    OracleCommand comm = new OracleCommand("select  distinct  APPROVAL_STATUS  from  rbavari.pov_purchaseOrderMaster", con);
                    OracleDataAdapter da = new OracleDataAdapter(comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox3.DataSource = ds.Tables[0];
                    ListBox3.DataTextField = ds.Tables[0].Columns["APPROVAL_STATUS"].ToString();
                    ListBox3.DataValueField = ds.Tables[0].Columns["APPROVAL_STATUS"].ToString();
                    ListBox3.DataBind();

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
                    OracleCommand comm = new OracleCommand("select distinct TRXREF from " + Session["schema_name"] + "pov_purchaseOrderMaster where VENDOR_CODE IN('" + CustName + "') ", con);

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
            Response.AddHeader("Content-Disposition", "inline; filename=PurchaseOrder.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
    }
}