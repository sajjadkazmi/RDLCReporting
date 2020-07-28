<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication2.Action.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rb Avari </title>
  <link rel='stylesheet' href='http://codepen.io/assets/libs/fullpage/jquery-ui.css' />
<link href="../css/style.css" rel="stylesheet" />
</head>
<body style="margin: 100px;">
 <div class="login-card" >
         <h1>Log-in</h1><br>
    <form id="form1" runat="server" >
 
    <asp:TextBox ID="txtpasscode" runat="server" CssClass="form-control" placeholder="Enter Pass Code"
        required="required" />
    <br />

<%--        <asp:CheckBox ID="chkRememberMe" Text="Remember Me" runat="server" />--%>
    <asp:Button ID="btnLogin" Text="Login" runat="server"  Class="login login-submit" OnClick="btnLogin_Click" />
</form>
    <br />
    <br />
    <div id="dvMessage" runat="server" visible="true" class="alert alert-danger">
        <strong></strong>
<%--        <asp:Label ID="lblMessage" runat="server" />--%>
        <asp:Label ID="lblMessage" runat="server" Text="" style="color:red;"></asp:Label>
    </div>
     <br />
  <%--  <div class="login-help">
    <a href="#">Register</a> • <a href="#">Forgot Password</a>
  </div>--%>
    </div >
      <script src='http://codepen.io/assets/libs/fullpage/jquery_and_jqueryui.js'></script>

</body>
</html>
