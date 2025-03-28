using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialMediaPlatform
{
    public partial class Home : System.Web.UI.Page
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
                LoadPosts();
            }
        }

        private void LoadPosts()
        {
            string query = @"
                SELECT p.PostID, u.FullName, p.Content, p.ImagePath, 
                       (SELECT COUNT(*) FROM Likes WHERE PostID = p.PostID) AS LikeCount
                FROM Posts p
                INNER JOIN Users u ON p.UserID = u.UserID
                ORDER BY p.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    rptPosts.DataSource = dt;
                    rptPosts.DataBind();
                }
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string content = txtPost.Text.Trim();
            string imagePath = null;

            if (fuImage.HasFile)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fuImage.FileName);
                string savePath = Server.MapPath("~/Uploads/") + fileName;
                fuImage.SaveAs(savePath);
                imagePath = "Uploads/" + fileName;
            }

            string query = "INSERT INTO Posts (UserID, Content, ImagePath, CreatedAt) VALUES (@UserID, @Content, @ImagePath, GETDATE())";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@ImagePath", (object)imagePath ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            txtPost.Text = ""; // Clear input after posting
            LoadPosts(); // Refresh posts
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx"); // Redirect to login page after logout
        }

        protected void btnLike_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            LinkButton btnLike = (LinkButton)sender;
            int postID = Convert.ToInt32(btnLike.CommandArgument);
            int userID = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                conn.Open();

                // Check if user already liked the post
                string checkQuery = "SELECT COUNT(*) FROM Likes WHERE UserID = @UserID AND PostID = @PostID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", userID);
                    checkCmd.Parameters.AddWithValue("@PostID", postID);
                    int likeExists = (int)checkCmd.ExecuteScalar();

                    if (likeExists == 0)
                    {
                        // Insert Like
                        string insertQuery = "INSERT INTO Likes (UserID, PostID) VALUES (@UserID, @PostID)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@UserID", userID);
                            insertCmd.Parameters.AddWithValue("@PostID", postID);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            LoadPosts(); // Refresh posts with updated like count
        }
    }
}
