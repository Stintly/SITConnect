<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password
    </title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Change Password<br />
            <br />
            New Password :&nbsp;
            <asp:TextBox ID="tb_newpassword" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Changepasswordbtn" runat="server" Text="Submit" Width="115px" OnClick="passwordchange"/>
            <br />
        </div>
    </form>
</body>
</html>
