using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.RBAVARI.PR
{
    public partial class PayrollEmpDetails : System.Web.UI.Page
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
                BindListbox();
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
            string ProcessMonth = ListBox2.SelectedItem.ToString();
            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", ListBoxValues), ProcessMonth);

            ReportDataSource rds = new ReportDataSource("PayrollData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/PayrollEmpDetail.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",ListBoxValues),
                    new ReportParameter ("Date",ProcessMonth),
                    new ReportParameter("USERID",Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;
        }

        private DataTable GetData(string Name, string Date)
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
                OracleDataAdapter da = new OracleDataAdapter("select old_Emp_no || '-' || employee_no emp_no, Company_Name, employee_name, father_spouse_name, cnic, appointment_date, confirmation_Date, left_date, extension_date, days_Worked, employment_status, department, designation, grade, worklocation, city, regionname, employee_bank, employee_bank_branchname, compensation, nvl(allowance, 0) - nvl(deduction, 0) amt from " + Session["schema_name"] + "prv_employeesalaryactl a where a.Process_Month = '" + Date + "' AND department IN  ('" + Name + "') ", con);
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
                    OracleCommand comm = new OracleCommand("select  distinct department from " + Session["schema_name"] + "prv_employeesalaryactl", con);
                    OracleDataAdapter daa = new OracleDataAdapter(comm);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox1.DataSource = dss.Tables[0];
                    ListBox1.DataTextField = dss.Tables[0].Columns["department"].ToString();
                    ListBox1.DataValueField = dss.Tables[0].Columns["department"].ToString();
                    ListBox1.DataBind();
                    //listbox2
                    OracleCommand com = new OracleCommand("select distinct Process_Month from " + Session["schema_name"] + "prv_employeesalaryactl order by Process_Month DESC", con);
                    OracleDataAdapter da = new OracleDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox2.DataSource = ds.Tables[0];
                    ListBox2.DataTextField = ds.Tables[0].Columns["Process_Month"].ToString();

                    ListBox2.DataValueField = ds.Tables[0].Columns["Process_Month"].ToString();
                    ListBox2.DataTextFormatString = "{0:dd-MMM-yyyy}";
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
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=MyReport.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            string sourcePdfPath = @"C:\Users\sajja\Desktop\file\MyReport.PDF";
            string DestinationFolder = @"C:\Users\sajja\Desktop\file";
            ExtractPages(sourcePdfPath, DestinationFolder);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");
        }
        public int ExtractPages(string sourcePdfPath, string DestinationFolder)
        {
            int p = 0;
            try
            {
                iTextSharp.text.Document document;
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(new iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdfPath), new ASCIIEncoding().GetBytes(""));
                if (!Directory.Exists(sourcePdfPath.ToLower().Replace(".pdf", "")))
                {
                    Directory.CreateDirectory(sourcePdfPath.ToLower().Replace(".pdf", ""));
                }
                else
                {
                    Directory.Delete(sourcePdfPath.ToLower().Replace(".pdf", ""), true);
                    Directory.CreateDirectory(sourcePdfPath.ToLower().Replace(".pdf", ""));
                }

                for (p = 1; p <= reader.NumberOfPages; p++)
                {

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        document = new iTextSharp.text.Document();
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                        writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_2);
                        writer.CompressionLevel = iTextSharp.text.pdf.PdfStream.BEST_COMPRESSION;
                        writer.SetFullCompression();
                        document.SetPageSize(reader.GetPageSize(p));
                        document.NewPage();
                        document.Open();
                        document.AddDocListener(writer);
                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                        iTextSharp.text.pdf.PdfImportedPage pageImport = writer.GetImportedPage(reader, p);
                        int rot = reader.GetPageRotation(p);
                        if (rot == 90 || rot == 270)
                        {
                            cb.AddTemplate(pageImport, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(p).Height);
                        }
                        else
                        {
                            cb.AddTemplate(pageImport, 1.0F, 0, 0, 1.0F, 0, 0);
                        }
                        document.Close();
                        document.Dispose();
                        File.WriteAllBytes(DestinationFolder + "/" + p + ".pdf", memoryStream.ToArray());
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
            return p - 1;

        }
    }
}