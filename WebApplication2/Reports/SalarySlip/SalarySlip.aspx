<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalarySlip.aspx.cs" Inherits="WebApplication2.Reports.SalarySlip.SalarySlip" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Show Report"  OnClick="ShowReport"/>

    <asp:Button ID="Button2" runat="server" Text="Download" OnClick="DownloadReport"/>

    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="1000px" Width="1100px">

    </rsweb:ReportViewer>
</asp:Content>
