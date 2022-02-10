<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyOTP.aspx.cs" Inherits="SITConnect.VerifyOTP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <p>Enter OTP that was sent to your email</p>
        <p>Verification code<asp:TextBox ID ="tb_OTPCode" runat="server" Height="25px" Width="137px" /></p>
        <p><asp:Button ID ="btnSubmit" runat="server" Text="Validate code" OnClick="VerifyCode" Height="27px" Width="133px" />
        <br />
        <br />
        <asp:Label ID="lblMessage" runat="server" EnabledViewState="False" ForeColor="Red"></asp:Label>
        </p>
       
        </div>
    </form>
</body>
</html>
