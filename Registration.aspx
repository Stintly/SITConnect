<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Registration</title>
    <script type ="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
            if (str.length == 0) {
                document.getElementById("lbl_suggest").innerHTML = "Password cannot be empty!"
                document.getElementById("lbl_suggest").style.color = "Red";
                return ("empty");
            }
            if (str.length < 12) {
                document.getElementById("lbl_suggest").innerHTML = "Password length must be at least 12 characters"
                document.getElementById("lbl_suggest").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_suggest").innerHTML = "Password requires at least 1 number"
                document.getElementById("lbl_suggest").style.color = "Red";
                return ("too_number");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_suggest").innerHTML = "Password requires at least 1 lowercase letter"
                document.getElementById("lbl_suggest").style.color = "Red";
                return ("need_lowercase");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_suggest").innerHTML = "Password requires at least 1 uppercase letter"
                document.getElementById("lbl_suggest").style.color = "Red";
                return ("need_uppercase");
            }
            else if (str.search(/[!@#$%&?]/) == -1) {
                document.getElementById("lbl_suggest").innerHTML = "Password requires at least 1 special charcater"
                document.getElementById("lbl_suggest").style.color = "Red";
                return ("need_special");
            }

            document.getElementById("lbl_suggest").innerHTML = "Excellent!"
            document.getElementById("lbl_suggest").style.color = "Blue";


        }
    </script>
</head>
<body style="height: 260px">
    <form id="form1" runat="server">
        <div>
            Registration<br />
            <br />
            First Name :&nbsp;
            <asp:TextBox ID="tb_firstname" runat="server" Width="360px" CausesValidation="True"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_firstname" ErrorMessage="Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            Last Name :&nbsp;
            <asp:TextBox ID="tb_lastname" runat="server" Width="360px"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tb_lastname" ErrorMessage="Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            Credit Card No. :&nbsp;
            <asp:TextBox ID="tb_creditnum" runat="server" Width="353px"></asp:TextBox>
            &nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tb_creditnum" ErrorMessage="Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            Credit Card Date:
            <asp:TextBox ID="tb_creditdate" runat="server" Width="327px"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tb_creditdate" ErrorMessage="Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            CVV :&nbsp;
            <asp:TextBox ID="tb_creditcvv" runat="server" Width="327px"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tb_creditcvv" ErrorMessage="Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
            Email Adresss :&nbsp;
            <asp:TextBox ID="tb_email" runat="server" Width="327px"></asp:TextBox>
            &nbsp;
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_email" ErrorMessage="Email is in the wrong format" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            <br />
            <br />
            Date of birth :
            <asp:TextBox ID="tb_dob" runat="server" Width="327px" type="date"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tb_dob" ErrorMessage="Cannot be empty" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <br />
        </div>
        <div id="row">
                    <asp:Label ID="photoLabel" runat="server" Text="Photo"></asp:Label>
                    <br />
                    <asp:FileUpload ID="photoTB" runat="server" />
                    <asp:Label ID="photoError" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="photoTB" ErrorMessage="*Photo is empty" ForeColor="Red"></asp:RequiredFieldValidator>
                    <br />
                </div>
        <p>
            Password&nbsp;:&nbsp;&nbsp;&nbsp; <asp:TextBox ID="tb_password" runat="server" Width="363px" type="password" onkeyup="javascript:validate()"></asp:TextBox>
&nbsp;&nbsp;
            <asp:Label ID="lbl_suggest" runat="server" Text="Label"></asp:Label>
        </p>
        <p>
            <asp:Label ID="lbl_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
        </p>
        <p>
            &nbsp;</p>
        <p>
            <asp:Button ID="chkpwd_btn" runat="server" Text="Check Password" OnClick ="btn_checkPassword_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="regsubmit_btn" runat="server" Text="Submit" OnClick="btn_Submit_Click"/>
        </p>
    </form>
</body>
</html>
