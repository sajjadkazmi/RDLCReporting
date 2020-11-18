<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalaryRegister.aspx.cs" Inherits="WebApplication2.RBAVARI.PR.SalaryRegister" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h2>Payroll Employee Details</h2>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" for="usr"> <h4> Select Department Name :</h4> </asp:Label>
                <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="multiple" ClientIDMode="Static"></asp:ListBox>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" for="usr"> <h4> Select Process Month :</h4> </asp:Label>
                <asp:ListBox ID="ListBox2" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
            </td>
            <td style="padding-top: 38px; padding-left: 20px">
                <asp:Button ID="Button2" class="btn btn-primary" runat="server" OnClick="Button1_Click" Text="View Report" />

            </td>
        </tr>
    </table>

    <br />

    <div>
        <asp:Button ID="PrintButton" class="btn btn-primary" runat="server" Text="Print" Visible="false" OnClick="Button2_Click" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1000" Height="1000"></rsweb:ReportViewer>
    </div>
</asp:Content>
