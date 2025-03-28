<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myprofile.aspx.cs" Inherits="SocialMediaPlatform.MyProfile" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>My Profile</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function toggleEditMode(editMode) {
            document.getElementById('<%= txtUsername.ClientID %>').disabled = !editMode;
            document.getElementById('<%= txtFullName.ClientID %>').disabled = !editMode;
            document.getElementById('<%= txtBio.ClientID %>').disabled = !editMode;
            document.getElementById('<%= fuProfileImage.ClientID %>').style.display = editMode ? "block" : "none";
            document.getElementById('<%= btnUpdate.ClientID %>').style.display = editMode ? "block" : "none";
            document.getElementById("btnEdit").style.display = editMode ? "none" : "block";
        }

        function previewImage() {
            var file = document.getElementById('<%= fuProfileImage.ClientID %>').files[0];
            if (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgProfile.ClientID %>').src = e.target.result;
                }
                reader.readAsDataURL(file);
            }
        }
    </script>
    <style>
        .profile-container {
            max-width: 600px;
            margin: auto;
            padding: 20px;
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            margin-top: 50px;
        }
        .profile-header {
            text-align: center;
            margin-bottom: 20px;
        }
        .profile-img {
            width: 120px;
            height: 120px;
            border-radius: 50%;
            border: 3px solid #ddd;
        }
    </style>
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="profile-container">
            <h2 class="profile-header">My Profile</h2>

            <asp:Label ID="lblMessage" runat="server" CssClass="text-success d-block text-center"></asp:Label>

            <div class="text-center">
                <!-- Profile Image -->
                <asp:Image ID="imgProfile" runat="server" CssClass="profile-img" />
                <div class="mt-2">
                    <asp:FileUpload ID="fuProfileImage" runat="server" CssClass="form-control" onchange="previewImage()" style="display: none;" />
                </div>
            </div>

            <div class="mt-4">
                <!-- Username -->
                <div class="mb-3">
                    <label class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Disabled="true"></asp:TextBox>
                </div>

                <!-- Full Name -->
                <div class="mb-3">
                    <label class="form-label">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Disabled="true"></asp:TextBox>
                </div>

                <!-- Email -->
                <div class="mb-3">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>

                <!-- Bio -->
                <div class="mb-3">
                    <label class="form-label">Bio</label>
                    <asp:TextBox ID="txtBio" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" Disabled="true"></asp:TextBox>
                </div>

                <div class="text-center">
                    <!-- Edit Button -->
                    <button type="button" id="btnEdit" class="btn btn-warning mt-2" onclick="toggleEditMode(true)">Edit Profile</button>

                    <!-- Save Button (Initially Hidden) -->
                    <asp:Button ID="btnUpdate" runat="server" Text="Save Changes" CssClass="btn btn-primary mt-2" OnClick="btnUpdate_Click" Style="display: none;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
