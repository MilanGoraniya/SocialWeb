<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SocialMediaPlatform.Home" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Home - Social Web</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="styles.css">
    <script>
        function previewImage() {
            var file = document.getElementById('fuImage').files[0];
            if (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('imgPreview').src = e.target.result;
                    document.getElementById('imgPreview').style.display = 'block';
                }
                reader.readAsDataURL(file);
            }
        }
    </script>
</head>
<body>

    <!-- ✅ ASP.NET FORM -->
    <form id="form1" runat="server">

        <!-- ✅ Navbar -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
            <div class="container">
                <a class="navbar-brand" href="#">🌍 Social Web</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <asp:LinkButton ID="btnProfile" runat="server" CssClass="nav-link" PostBackUrl="~/myprofile.aspx">👤 My Profile</asp:LinkButton>
                        </li>
                        <li class="nav-item">
                            <asp:LinkButton ID="btnLogout" runat="server" CssClass="nav-link" OnClick="btnLogout_Click">🚪 Logout</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- ✅ Main Content -->
        <div class="container mt-4">
            <!-- Create Post Section -->
            <div class="post-box">
                <h4>Create a Post</h4>
                <asp:TextBox ID="txtPost" runat="server" CssClass="form-control" placeholder="What's on your mind?" TextMode="MultiLine"></asp:TextBox>

                <!-- File Upload + Image Preview -->
                <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control mt-2" onchange="previewImage()" />
                <img id="imgPreview" src="#" alt="Image Preview" class="img-fluid mt-2" style="display: none; max-width: 100%; border-radius: 10px;" />
                <asp:Button ID="btnPost" runat="server" Text="Post" CssClass="btn btn-post mt-3 w-100" OnClick="btnPost_Click" />
            </div>

            <!-- ✅ Display Posts -->
            <asp:Repeater ID="rptPosts" runat="server">
              <ItemTemplate>
    <div class="post-box">
        <p><strong><%# Eval("FullName") %>:</strong> <%# Eval("Content") %></p>

        <!-- ✅ Corrected Image Display Logic -->
        <asp:Panel runat="server" Visible='<%# Eval("ImagePath") != DBNull.Value && !string.IsNullOrEmpty(Eval("ImagePath").ToString()) %>'>
            <div class="image-container">
                <asp:Image ID="imgPost" runat="server" CssClass="img-fluid" ImageUrl='<%# Eval("ImagePath") %>' />
            </div>
        </asp:Panel>

        <!-- ✅ Like Button & Like Count -->
        <div class="like-section mt-2">
            <asp:LinkButton ID="btnLike" runat="server" CssClass="btn btn-primary" CommandArgument='<%# Eval("PostID") %>' OnClick="btnLike_Click">
                👍 Like
            </asp:LinkButton>
            <span class="like-count">Likes: <%# Eval("LikeCount") %></span>
        </div>
    </div>
</ItemTemplate>


            </asp:Repeater>
        </div>

    </form>

</body>
</html>
