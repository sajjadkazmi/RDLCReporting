<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invoices.aspx.cs" Inherits="WebApplication2.RBAVARI.SO.Invoices" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-group">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" for="usr"> <h4>Select Customer Name :</h4> </asp:Label>
                    <asp:ListBox ID="ListBox2" runat="server" class="form-control" SelectionMode="single" ClientIDMode="Static"></asp:ListBox>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" for="usr"> <h4>Select Invoice No:</h4> </asp:Label>
                    <asp:Button ID="Button2" class="btn btn-primary" runat="server" OnClick="Button2_Click" Text="Select"></asp:Button>
                    <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
                </td>
                <td style="padding-top: 38px; padding-left: 20px">
                    <asp:Button ID="Button1" class="btn btn-primary" runat="server" OnClick="Button1_Click" Text="View Report" />
                </td>
            </tr>
        </table>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div id="ReportViewer1_ctl10">
        <asp:Button ID="PrintButton" class="btn btn-primary" runat="server" Text="Print" Visible="false" OnClick="PrintButton_Click" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Width="1200px"></rsweb:ReportViewer>
    </div>
</asp:Content>
