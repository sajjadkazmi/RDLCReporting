<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BankAdvice.aspx.cs" Inherits="WebApplication2.RBAVARI.PR.BankAdvice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" for="usr"> <h4> Select Employee Type :</h4> </asp:Label>
                <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" for="usr"> <h4>Mode of Payment :</h4> </asp:Label>
                <asp:ListBox ID="ListBox2" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
            </td>
            </tr>
        <tr>
                        <td>
                <asp:Label ID="Label4" runat="server" for="usr"> <h4>Work Location:</h4> </asp:Label>
                <asp:ListBox ID="ListBox4" runat="server" class="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" for="usr"> <h4> Select Process Month :</h4> </asp:Label>
                <asp:ListBox ID="ListBox3" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
            </td>
        </tr>
    </table>

    <br />
    <asp:Button ID="Button1" class="btn btn-primary" runat="server" OnClick="Button1_Click" Text="View Report" />
    <br />

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Button ID="PrintButton" class="btn btn-primary" runat="server" Text="Print" Visible="false" OnClick="PrintButton_Click" />
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1000px"></rsweb:ReportViewer>
</asp:Content>
