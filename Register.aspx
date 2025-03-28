<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SocialMediaPlatform.Register" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Register - Social Media</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="styles.css">
</head>
<body>

<div class="form-container">
    <h3 class="text-center">Join Us</h3>
    <p class="text-center text-muted">Create a new account</p>

    <form runat="server">
        <div class="mb-3">
            <label>Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <div class="mb-3">
            <label>Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <div class="mb-3">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Sign Up" CssClass="btn btn-custom w-100" OnClick="btnRegister_Click" />
        
        <p class="text-center mt-3">Already have an account? <a href="Login.aspx">Log in</a></p>
    </form>
</div>

</body>
</html>
