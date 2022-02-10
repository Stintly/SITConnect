<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form</title>
        <script src="https://www.google.com/recaptcha/api.js?render=6Lcxk2UeAAAAAHspn8LbjUyESYjBbI_CbPTSUwv5"></script>
</head>
<body style="height: 217px">
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            Username :&nbsp;
            <asp:TextBox ID="tb_userid" runat="server"></asp:TextBox>
            <br />
            <br />
            Password :&nbsp;
            <asp:TextBox ID="tb_pwd" runat="server" type="password"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btn_login" runat="server" Text="Login" OnClick ="LoginMe"/>
            <br />
            <br />
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>

            <asp:Label ID="lblMessage" runat="server" Text="Error Message here">Error Message here</asp:Label>
            
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lcxk2UeAAAAAHspn8LbjUyESYjBbI_CbPTSUwv5', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
