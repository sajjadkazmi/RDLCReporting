<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PO.aspx.cs" Inherits="WebApplication2.Reports.PurchaseOrder.PO" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-group">
            <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" for="usr"> <h4> Select Order No :</h4> </asp:Label>
                <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="multiple" ClientIDMode="Static"></asp:ListBox>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" for="usr"> <h4> Select Status :</h4> </asp:Label>
                <asp:ListBox ID="ListBox2" runat="server" class="form-control" SelectionMode="Single" ClientIDMode="Static"></asp:ListBox>
            </td>
           <td style=" padding-top: 38px; padding-left:20px">
      <asp:Button ID="Button2" class="btn btn-primary" runat="server" OnClick="Button1_Click" Text="View Report" />
           </td>
        </tr>
    </table>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div id="ReportViewer1_ctl10">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Width="1000px">
        </rsweb:ReportViewer>
    </div>

    <style type="text/css">
        .multiselect-container {
            width: 600px !important;
        }
    </style>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#ListBox1,#ListBox2').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '400px',
                maxHeight: 300
            });
        });

    </script>


</asp:Content>
