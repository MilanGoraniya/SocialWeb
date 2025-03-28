<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SocialMediaPlatform.Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Login - Social Media</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="styles.css">
</head>
<body>

<div class="form-container">
    <h3 class="text-center">Welcome Back</h3>
    <p class="text-center text-muted">Log in to your account</p>
    
    <form runat="server">
        <div class="mb-3">
            <label>Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <div class="mb-3">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-custom w-100" OnClick="btnLogin_Click" />
        
        <p class="text-center mt-3">Don't have an account? <a href="Register.aspx">Sign up</a></p>
    </form>
</div>

</body>
</html>
