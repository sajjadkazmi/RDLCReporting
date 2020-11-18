<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GroupInsurance.aspx.cs" Inherits="WebApplication2.LCPW.PR.GroupInsurance" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
         <div class="form-group" >

                    <table>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" for="usr"> <h4>Select Status:</h4> </asp:Label>
                <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="multiple" ClientIDMode="Static"></asp:ListBox>
            </td>
            </tr>
                        <tr>
           
           <td style=" padding-top: 38px; padding-left:20px">
     <asp:Button ID="Button1" class="btn btn-primary" runat="server" OnClick="Button1_Click" Text="View Report" /> 
           </td>
        </tr>
    </table>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>

    <div ID="ReportViewer1_ctl10">
        <asp:Button ID="PrintButton" class="btn btn-primary" runat="server" Text="Print" Visible="false" OnClick="PrintButton_Click" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Width="1000px"></rsweb:ReportViewer>
        </div>
</asp:Content>
