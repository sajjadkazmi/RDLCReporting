using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Reports.PaySlip
{
    public partial class PaySlip : System.Web.UI.Page
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

            string ListBoxValues = "";
            string value = "";
            foreach (int i in ListBox1.GetSelectedIndices())
            {
                value = value + "'" + ListBox1.Items[i].Value + "',";
                ListBoxValues = string.Join(" ", value.Split(' ').Select(x => x.Trim('\''))).TrimEnd(',').TrimEnd('\'');
            }
            //Reset
            ReportViewer1.Reset();
            //datasource
            DataTable dt = GetData(string.Join(" ", ListBoxValues));

            ReportDataSource rds = new ReportDataSource("PaySlipData", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);
            //path

            ReportViewer1.LocalReport.ReportPath = "./Reports/PaySlip/Report6.rdlc";
            //parameter
            ReportParameter[] rptParms = new ReportParameter[]
            {
                    new ReportParameter ("Name",ListBoxValues),
                    new ReportParameter("USERID",Session["u_id"].ToString(),false)
            };
            ReportViewer1.LocalReport.SetParameters(rptParms);
            //refresh
            ReportViewer1.LocalReport.Refresh();
      
        }

        private DataTable GetData(string Name)
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
                OracleDataAdapter da = new OracleDataAdapter("select old_Emp_no || '-' || employee_no emp_no, Company_Name, employee_name, father_spouse_name, cnic, appointment_date, confirmation_Date, left_date, extension_date, days_Worked, employment_status, department, designation, grade, worklocation, city, regionname, employee_bank, employee_bank_branchname, compensation, nvl(allowance, 0) - nvl(deduction, 0) amt from " + Session["schema_name"] + "prv_employeesalaryactl a where a.Process_Month = '31-Jul-10' AND emp_no IN  ('" + Name + "') ", con);
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
                    OracleCommand comm = new OracleCommand("select  distinct old_Emp_no || '-' || employee_no emp_no from " + Session["schema_name"] + "prv_employeesalaryactl", con);
                    OracleDataAdapter daa = new OracleDataAdapter(comm);
                    DataSet dss = new DataSet();
                    daa.Fill(dss); // fill dataset
                    ListBox1.DataSource = dss.Tables[0];
                    ListBox1.DataTextField = dss.Tables[0].Columns["emp_no"].ToString();
                    ListBox1.DataValueField = dss.Tables[0].Columns["emp_no"].ToString();
                    ListBox1.DataBind();
                    //listbox2
           
                }
                con.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        //private void bindreport()
        //{
        //    Connection getCon = new Connection();
        //    string connectString = getCon.create_connection();
        //    OracleConnection con = new OracleConnection(connectString);

        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        OracleDataAdapter da = new OracleDataAdapter("select old_Emp_no || '-' || employee_no emp_no, Company_Name, employee_name, father_spouse_name, cnic, appointment_date, confirmation_Date, left_date, extension_date, days_Worked, employment_status, department, designation, grade, worklocation, city, regionname, employee_bank, employee_bank_branchname, compensation, nvl(allowance, 0) - nvl(deduction, 0) amt from rbavari.prv_employeesalaryactl a where a.Process_Month = '31-Jul-2019' ", con);

        //    DataTable dt = new DataTable("DemoDt");
        //    da.Fill(dt);

        //    ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //    ReportViewer1.LocalReport.DataSources.Clear();

        //    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PaySlipData", dt));
        //    //ReportViewer1.LocalReport.Refresh();
        //    con.Close();

        //}

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    bindreport();
        //}
    }
}