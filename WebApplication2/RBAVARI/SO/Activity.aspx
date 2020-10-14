<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="WebApplication2.RBAVARI.SO.Activity" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <div class="form-group">

        <table>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" for="usr"> <h4>Select Customer Name :</h4> </asp:Label>
                    <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                </td>
           
<%--                <td>
                    <div class="input-group">
                    <asp:Label ID="Label2" runat="server" for="usr"> <h4>From Date:</h4> </asp:Label>
                    <asp:TextBox id="datepicker" class="form-control datePicker" placeholder="Select Date" autocomplete="off" runat="server"/>
                    </div>
                    
                </td>--%>
                <td>
                    <div class="input-group">
                    <asp:Label ID="Label1" runat="server" for="usr"> <h4>To Date:</h4> </asp:Label>
                    <asp:TextBox id="datepicker2" class="form-control datePicker" placeholder="Select Date" autocomplete="off" runat="server"/>
                    </div>
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
