<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaySlip.aspx.cs" Inherits="WebApplication2.LCPW.PR.PaySlip" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">

         <table>
        <tr>
            <td>
        <asp:Label ID="Label1" runat="server" for="usr"> <h4> Select Process Month:</h4> </asp:Label>
        <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
                </td>
            <td>
        <asp:Label ID="Label3" runat="server" for="usr"> <h4> Select Employee Name:</h4> </asp:Label>
        <asp:ListBox ID="ListBox2" runat="server" class="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>

                </td>

            </tr>
             </table>
        <asp:Button ID="Button1" class="btn btn-primary" runat="server" OnClick="Button1_Click" Text="View Report" />
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div id="ReportViewer1_ctl10">
<%--        <asp:Button ID="Button2" class="btn btn-primary" runat="server" OnClick="DownloadReport" Text="Download" Visible="false" />--%>
      
        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Width="1000px"></rsweb:ReportViewer>
    </div>
</asp:Content>
