<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SITConnect.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page</title>
    <style>
body {
  margin: 0;
  font-family: Arial, Helvetica, sans-serif;
}

.topnav {
  overflow: hidden;
  background-color: #333;
}

.topnav a {
  float: left;
  color: #f2f2f2;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
  font-size: 17px;
}

.topnav a:hover {
  background-color: #ddd;
  color: black;
}

.topnav a.active {
  background-color: #04AA6D;
  color: white;
}
</style>
</head>
<body style="height: 236px">

<div class="topnav">
  <a class="active" href="HomePage.aspx">Home</a>
  <a href="ChangePassword.aspx">Change Password</a>
  <a href="#contact">Contact</a>
  <a href="#about">About</a>
</div>

    <form id="form1" runat="server">
        <div>
            
            <br />
            Email :
            <asp:Label ID="lbl_useremail" runat="server" Text="Label"></asp:Label>
            <br />
            
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <br />
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick ="LogoutMe"/>
            
        </div>
    </form>
</body>
</html>
