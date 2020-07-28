using iTextSharp.text.pdf.parser;
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

namespace WebApplication2.Reports.SalarySlip
{
    public partial class SalarySlip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void bindreport()
        {
            Connection getCon = new Connection();
            string connectString = getCon.create_connection();
            OracleConnection con = new OracleConnection(connectString);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            OracleDataAdapter da = new OracleDataAdapter("select old_Emp_no || '-' || employee_no emp_no, Company_Name, employee_name, father_spouse_name, cnic, appointment_date, confirmation_Date, left_date, extension_date, days_Worked, employment_status, department, designation, grade, worklocation, city, regionname, employee_bank, employee_bank_branchname, compensation, nvl(allowance, 0) - nvl(deduction, 0) amt from rbavari.prv_employeesalaryactl a where a.Process_Month = '31-Jul-2019' ", con);

            DataTable dt = new DataTable("DemoDt");
            da.Fill(dt);

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("SalarySlipData", dt));
            ReportViewer1.LocalReport.ReportPath = "./Reports/SalarySlip/Report7.rdlc";
            ReportViewer1.LocalReport.Refresh();
            //con.Close();

        }

        protected void DownloadReport(object sender, EventArgs e)
        {
            string Url = ConvertReportToPDF(ReportViewer1.LocalReport);
            string sourcePdfPath = Url;
        
            //string DestinationFolder = @"C:\Users\sajja\Desktop\file";

            string DestinationFolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            ExtractPages(sourcePdfPath, DestinationFolder);
            System.Diagnostics.Process.Start(Url);

            File.Delete(Url);
        }

        private string ConvertReportToPDF(LocalReport rep)
        {
            string reportType = "PDF";
            string mimeType;
            string encoding;

            string deviceInfo = "<DeviceInfo>" +
               "  <OutputFormat>PDF</OutputFormat>" +
               "  <PageWidth>8.27in</PageWidth>" +
               "  <PageHeight>6.0in</PageHeight>" +
               "  <MarginTop>0.2in</MarginTop>" +
               "  <MarginLeft>0.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.2in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streamIds;
            string extension = string.Empty;

            byte[] bytes = rep.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
            string localPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            //string localPath = AppDomain.CurrentDomain.BaseDirectory+"pdfiles";
       

            ////string fileName = Guid.NewGuid().ToString() + ".pdf";
            string fileName = "MyReport" + ".pdf";
            localPath = localPath+ @"\" + fileName;
            System.IO.File.WriteAllBytes(localPath, bytes);
            return localPath;
        }

        protected void ShowReport(object sender, EventArgs e)
        {
            bindreport();
        }

        public int ExtractPages(string sourcePdfPath, string DestinationFolder)
        {
            int p = 0;
            string strText = string.Empty;
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

                    string dirPath = DestinationFolder.ToLower().Replace(".pdf", "");
                    string sub = dirPath.Substring(dirPath.IndexOf("Downloads") + 26);
                    string dirName = sub.Substring(0, 8);
                }

                for (p = 1; p <= reader.NumberOfPages; p++)
                {
                    ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(reader, p, strategy);
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    //strText = strText + currentText;

                    string sub = currentText.Substring(currentText.IndexOf("Employee ID:") + 13);
                    string EmpID = sub.Substring(0, 11);

                    //string dirPath = sourcePdfPath.ToLower().Replace(".pdf", "");
                    //string subb = dirPath.Substring(dirPath.IndexOf("Downloads") + 26);
                    //string dirName = subb.Substring(0, 8);

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
                        File.WriteAllBytes(DestinationFolder +"/"+ dirName+"/" + EmpID + p+ ".pdf", memoryStream.ToArray());
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            catch (Exception)
            {
                throw; 
            }   
            finally
            {
                GC.Collect();
            }
            return p - 1;

        }
    }

}