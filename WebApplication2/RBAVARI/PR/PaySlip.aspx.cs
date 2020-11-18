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

namespace WebApplication2.RBAVARI.PR
{
    public partial class PaySlip : System.Web.UI.Page
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
                BindListbox2();
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            showReport();
        }

        private void showReport()
        {

            //string Region = "";
            //string value = "";
            //foreach (int i in ListBox2.GetSelectedIndices())
            //{
            //    value = value + "'" + ListBox2.Items[i].Value + "',";
            //    Region = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            //}

            string EmpName = "";
            string value2 = "";
            foreach (int i in ListBox2.GetSelectedIndices())
            {
                value2 = value2 + "'" + ListBox2.Items[i].Value + "',";
                EmpName = string.Join(" ", value2.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            string Date = ListBox1.SelectedValue.ToString().Substring(0, 10);
            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(Date,EmpName);

            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            //Update DataTable with barcode image
            foreach (PaySlipData.DataTable1Row row in dt.Rows)
            {
                //Set the value to encode
                //bcp.Code = row.TRXREF.ToString();
                QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(row.emp_no.ToString(), QRCoder.QRCodeGenerator.ECCLevel.Q);
                QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);
                Bitmap bmp = qRCode.GetGraphic(5);

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Bmp);
                    row.BarCode = ms.ToArray();
                    //reportData.QRCode.AddQRCodeRow(qRCodeRow);
                }
            }

            ReportDataSource rds = new ReportDataSource("PaySlipData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/PaySlip.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",Date),
                    new ReportParameter("USERID", Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
            //Button2.Visible = true;
        }

        private DataTable GetData(string Date,string EmpName)
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
                OracleDataAdapter da = new OracleDataAdapter("select old_Emp_no || '-' || employee_no emp_no,employee_no, Company_Name, employee_name, father_spouse_name, cnic, appointment_date, confirmation_Date, left_date, extension_date, days_Worked, employment_status, department, designation, grade, worklocation, city, regionname, employee_bank, employee_bank_branchname, compensation, nvl(allowance, 0) - nvl(deduction, 0) amt,allowance,deduction,net from rbavari.prv_employeesalaryactl a where a.Process_Month = to_date('" + Date + "','mm/dd/rrrr') AND employee_no IN ('" + EmpName+ "')", con);
                DataTable dtt = new DataTable("DemoDt");

                PaySlipData.DataTable1DataTable dt = new PaySlipData.DataTable1DataTable();
                da.Fill(dt);
                con.Close();
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
                    OracleCommand comm = new OracleCommand("Select distinct Process_Month from rbavari.prv_employeesalaryactl order by Process_Month DESC", con);
                    OracleDataAdapter da = new OracleDataAdapter(comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox1.DataSource = ds.Tables[0];
                    ListBox1.DataTextField = ds.Tables[0].Columns["Process_Month"].ToString();
                    ListBox1.DataValueField = ds.Tables[0].Columns["Process_Month"].ToString();
                    ListBox1.DataTextFormatString = "{0:dd-MMM-yyyy}";
                    ListBox1.DataBind();
                }
                con.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void BindListbox2()
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
                    OracleCommand comm = new OracleCommand("Select distinct(employee_no),employee_name from rbavari.prv_employeesalaryactl order by employee_name", con);
                    OracleDataAdapter da = new OracleDataAdapter(comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds); // fill dataset
                    ListBox2.DataSource = ds.Tables[0];
                    ListBox2.DataTextField = ds.Tables[0].Columns["employee_name"].ToString();
                    ListBox2.DataValueField = ds.Tables[0].Columns["employee_no"].ToString();
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
    

        public static string getHomePath()
        {
            // Not in .NET 2.0
            // System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return System.Environment.GetEnvironmentVariable("HOME");

            return System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }


        public static string getDownloadFolderPath()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                string pathDownload = System.IO.Path.Combine(getHomePath(), "Downloads");
                return pathDownload;
            }

            return System.Convert.ToString(
                Microsoft.Win32.Registry.GetValue(
                     @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    , "{374DE290-123F-4565-9164-39C4925E467B}"
                    , String.Empty
                )
            );
        }

        //protected void DownloadReport(object sender, EventArgs e)
        //{
           
        //    string Url = ConvertReportToPDF(ReportViewer1.LocalReport);
        //    //System.Diagnostics.Process.Start(Url);
        //    string sourcePdfPath = Url;
        //    string folderPath = System.IO.Path.ChangeExtension(sourcePdfPath.ToLower(), null);
        //    //File.Delete(folderPath);

        //    //string DestinationFolder = AppDomain.CurrentDomain.BaseDirectory;
        //    string DestinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    ExtractPages(sourcePdfPath, DestinationFolder);

        //    //System.Diagnostics.Process.Start(Url);
        //    File.Delete(Url);
        //    Label2.Text = folderPath;
        //}

        //private string ConvertReportToPDF(LocalReport rep)
        //{
        //    string reportType = "PDF";
        //    string mimeType;
        //    string encoding;

        //    string deviceInfo = "<DeviceInfo>" +
        //       "  <OutputFormat>PDF</OutputFormat>" +
        //       "  <PageWidth>8.27in</PageWidth>" +
        //       "  <PageHeight>6.0in</PageHeight>" +
        //       "  <MarginTop>0.2in</MarginTop>" +
        //       "  <MarginLeft>0.2in</MarginLeft>" +
        //       "  <MarginRight>0.2in</MarginRight>" +
        //       "  <MarginBottom>0.2in</MarginBottom>" +
        //       "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streamIds;
        //    string extension = string.Empty;

        //    byte[] bytes = rep.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //    //string localPath = AppDomain.CurrentDomain.BaseDirectory;
        //    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //    //string fileName = Guid.NewGuid().ToString() + ".pdf";
        //    string fileName = "MyReport" + ".pdf";
        //    localPath = localPath + @"\" + fileName;
        //    System.IO.File.WriteAllBytes(localPath, bytes);
        //    return localPath;
        //}

        //public int ExtractPages(string sourcePdfPath, string DestinationFolder)
        //{
        //    int p = 0;
        //    string strText = string.Empty;
        //    try
        //    {
        //        iTextSharp.text.Document document;
        //        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(new iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdfPath), new ASCIIEncoding().GetBytes(""));
        //        if (!Directory.Exists(sourcePdfPath.ToLower().Replace(".pdf", "")))
        //        {
        //            Directory.CreateDirectory(sourcePdfPath.ToLower().Replace(".pdf", ""));
        //        }
        //        else
        //        {
        //            Directory.Delete(sourcePdfPath.ToLower().Replace(".pdf", ""), true);
        //            Directory.CreateDirectory(sourcePdfPath.ToLower().Replace(".pdf", ""));

        //            //string dirPath = DestinationFolder.ToLower().Replace(".pdf", "");
        //            //string sub = dirPath.Substring(dirPath.IndexOf("Downloads") + 26);
        //            //string dirName = sub.Substring(0, 8);
        //        }

        //        for (p = 1; p <= reader.NumberOfPages; p++)
        //        {
        //            ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
        //            string currentText = PdfTextExtractor.GetTextFromPage(reader, p, strategy);
        //            currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    
        //            int pos1 = currentText.IndexOf("Employee ID") + 11;
        //            int pos2 = currentText.IndexOf("Department", pos1);
        //            string EmpID = currentText.Substring(pos1, pos2 - pos1).Trim();

        //            string dirPath = sourcePdfPath.ToLower().Replace(".pdf", "");
        //            string dirName = "myreport";

        //            using (MemoryStream memoryStream = new MemoryStream())
        //            {
        //                document = new iTextSharp.text.Document();
        //                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
        //                writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_2);
        //                writer.CompressionLevel = iTextSharp.text.pdf.PdfStream.BEST_COMPRESSION;
        //                writer.SetFullCompression();
        //                document.SetPageSize(reader.GetPageSize(p));
        //                document.NewPage();
        //                document.Open();
        //                document.AddDocListener(writer);
        //                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
        //                iTextSharp.text.pdf.PdfImportedPage pageImport = writer.GetImportedPage(reader, p);
        //                int rot = reader.GetPageRotation(p);
        //                if (rot == 90 || rot == 270)
        //                {
        //                    cb.AddTemplate(pageImport, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(p).Height);
        //                }
        //                else
        //                {
        //                    cb.AddTemplate(pageImport, 1.0F, 0, 0, 1.0F, 0, 0);
        //                }
        //                document.Close();
        //                document.Dispose();
        //                File.WriteAllBytes(DestinationFolder + "/" + dirName + "/" + EmpID + ".pdf", memoryStream.ToArray());
        //            }
        //        }
        //        reader.Close();
        //        reader.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //    return p - 1;

        //}
    }
}