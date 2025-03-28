using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SocialMediaPlatform
{
    public partial class MyProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            if (Session["UserID"] == null) return;

            int userID = Convert.ToInt32(Session["UserID"]);
            string query = "SELECT Username, FullName, Email, Bio, ProfileImage FROM Users WHERE UserID = @UserID";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUsername.Text = reader["Username"].ToString();
                            txtFullName.Text = reader["FullName"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtBio.Text = reader["Bio"] != DBNull.Value ? reader["Bio"].ToString() : "";

                            string imagePath = reader["ProfileImage"] != DBNull.Value
                                ? reader["ProfileImage"].ToString()
                                : "Uploads/default.png";

                            imgProfile.ImageUrl = imagePath;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading profile: " + ex.Message;
                lblMessage.CssClass = "text-danger";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) return;

            int userID = Convert.ToInt32(Session["UserID"]);
            string username = txtUsername.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string bio = txtBio.Text.Trim();
            string imagePath = imgProfile.ImageUrl; // Default to current image

            // ✅ Validate Username (optional, but good for security)
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                lblMessage.Text = "Username must be at least 3 characters long.";
                lblMessage.CssClass = "text-danger";
                return;
            }

            // ✅ Handle profile image upload securely
            if (fuProfileImage.HasFile)
            {
                string fileExt = Path.GetExtension(fuProfileImage.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                if (Array.Exists(allowedExtensions, ext => ext == fileExt))
                {
                    try
                    {
                        string fileName = Guid.NewGuid() + fileExt;
                        string savePath = Server.MapPath("~/Uploads/") + fileName;
                        fuProfileImage.SaveAs(savePath);
                        imagePath = "Uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error uploading file: " + ex.Message;
                        lblMessage.CssClass = "text-danger";
                        return;
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.";
                    lblMessage.CssClass = "text-danger";
                    return;
                }
            }

            string query = "UPDATE Users SET Username = @Username, FullName = @FullName, Bio = @Bio, ProfileImage = @ProfileImage WHERE UserID = @UserID";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Bio", string.IsNullOrEmpty(bio) ? DBNull.Value : (object)bio);
                    cmd.Parameters.AddWithValue("@ProfileImage", string.IsNullOrEmpty(imagePath) ? DBNull.Value : (object)imagePath);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMessage.Text = "Profile updated successfully!";
                lblMessage.CssClass = "text-success";
                LoadProfile(); // Refresh profile info
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating profile: " + ex.Message;
                lblMessage.CssClass = "text-danger";
            }
        }
    }
}
