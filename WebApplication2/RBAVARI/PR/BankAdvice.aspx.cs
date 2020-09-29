using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.RBAVARI.PR
{
    public partial class BankAdvice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["u_id"] == null)
            {
                Response.Redirect("~/Action/Login.aspx");
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

            //string ListBoxValues = "";
            //string value = "";
            //foreach (int i in ListBox1.GetSelectedIndices())
            //{
            //    value = value + "'" + ListBox1.Items[i].Value + "',";
            //    ListBoxValues = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',');
            //}
            string EmpBank = "";
            string value2 = "";
            foreach (int i in ListBox2.GetSelectedIndices())
            {
                value2 = value2 + "'" + ListBox2.Items[i].Value + "',";
                EmpBank = value2.TrimEnd(',').TrimEnd('\'').TrimStart('\'');
            }
            //string EmpBank = ListBox2.SelectedItem.ToString();
            string Name = ListBox1.SelectedItem.ToString();
            string processmonth = ListBox3.SelectedItem.ToString();

            var datetime = DateTime.Parse(processmonth);
            var ProcessMonth = datetime.ToString("dd-MMM-yyyy");

            //string ProcessMonth = ProcessMonth1.ToString();


            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(Name, EmpBank, ProcessMonth);

            ReportDataSource rds = new ReportDataSource("NetPayData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./RBAVARI/PR/BankAdvice.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",Name),
                    new ReportParameter ("EmpBank",EmpBank),
                    new ReportParameter ("Date",ProcessMonth),
                    new ReportParameter("USERID",Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
            PrintButton.Visible = true;

        }

        private DataTable GetData(string Name, string EmpBank, string ProcessMonth)
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
                OracleDataAdapter da = new OracleDataAdapter("select old_Emp_no || '-' || employee_no emp_no, Company_Name, employee_name, father_spouse_name, cnic,phone_no,addressline, appointment_date, confirmation_Date, left_date, extension_date, days_Worked, employment_status, department, designation, grade, worklocation, city, regionname, employee_bank, employee_bank_branchname,emp_bank_ac,net from rbavari.Prv_Employeesalaryactl a where a.Process_Month = '" + ProcessMonth + "' AND compensation_code='00-000' AND employment_status='" + Name + "' AND employee_bank_branchname IN  ('" + EmpBank + "') AND employee_no not in (select emp_no from rbavari.pr_holdsalary where process_month = '" + ProcessMonth + "')", con);
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
                    OracleCommand comm = new OracleCommand("select  distinct employment_status  from rbavari.prv_employeesalaryactl", con);
                    OracleDataAdapter daa = new OracleDataAdapter(comm);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox1.DataSource = dss.Tables[0];
                    ListBox1.DataTextField = dss.Tables[0].Columns["employment_status"].ToString();
                    ListBox1.DataValueField = dss.Tables[0].Columns["employment_status"].ToString();
                    ListBox1.DataBind();

                    //listbox2

                    OracleCommand com2 = new OracleCommand("select  distinct employee_bank_branchname  from rbavari.prv_employeesalaryactl", con);
                    OracleDataAdapter da2 = new OracleDataAdapter(com2);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2); // fill dataset
                    ListBox2.DataSource = ds2.Tables[0];
                    ListBox2.DataTextField = ds2.Tables[0].Columns["employee_bank_branchname"].ToString();
                    ListBox2.DataValueField = ds2.Tables[0].Columns["employee_bank_branchname"].ToString();
                    ListBox2.DataBind();

                    //listbox3

                    OracleCommand com3 = new OracleCommand("select  distinct Process_Month  from rbavari.prv_employeesalaryactl order by Process_Month", con);
                    OracleDataAdapter da3 = new OracleDataAdapter(com3);
                    DataSet ds3 = new DataSet();
                    da3.Fill(ds3); // fill dataset
                    ListBox3.DataSource = ds3.Tables[0];
                    ListBox3.DataTextField = ds3.Tables[0].Columns["Process_Month"].ToString();
                    ListBox3.DataValueField = ds3.Tables[0].Columns["Process_Month"].ToString();
                    ListBox3.DataBind();

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
            Response.AddHeader("Content-Disposition", "inline; filename=SalaryAdvice.PDF");
            Response.ContentType = "application/PDF";
            Response.BinaryWrite(bytes);
            Response.End();
            ClientScript.RegisterStartupScript(typeof(Page), "key", "<script type='text/javascript'>window.print();;</script>");

        }
    }
}