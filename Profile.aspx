<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SocialMediaPlatform.Profile" %>
<!DOCTYPE html>
<html>
<head>
    <title>User Profile</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Profile</h2>
        <asp:Image ID="imgProfile" runat="server" Width="100" Height="100" />
        <h3><asp:Label ID="lblName" runat="server"></asp:Label></h3>
        <h4>Email: <asp:Label ID="lblEmail" runat="server"></asp:Label></h4>
    </form>
</body>
</html>
