<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication2.Action.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rb Avari </title>
 <link href="~/css/style.css" rel="stylesheet" />
</head>
<body style="margin: 100px;">
 <div class="login-card" >
         <h1>Log-in</h1><br>
    <form id="form1" runat="server" >
 
    <asp:TextBox ID="txtpasscode" runat="server" CssClass="form-control" placeholder="Enter Pass Code"
        required="required" />
    <br />

    <asp:Button ID="btnLogin" Text="Login" runat="server"  Class="login login-submit" OnClick="btnLogin_Click" />
</form>
    <br />
    <br />
    <div id="dvMessage" runat="server" visible="true" class="alert alert-danger">
        <strong></strong>
        <asp:Label ID="lblMessage" runat="server" Text="" style="color:red;"></asp:Label>
    </div>
    </div >

</body>
</html>
