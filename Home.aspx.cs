using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
                {
                    string query = @"
                        SELECT p.PostID, u.FullName, p.Content, p.ImagePath, 
                               (SELECT COUNT(*) FROM Likes WHERE PostID = p.PostID) AS LikeCount
                        FROM Posts p
                        INNER JOIN Users u ON p.UserID = u.UserID
                        ORDER BY p.CreatedAt DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (rptPosts != null)
                        {
                            rptPosts.DataSource = dt;
                            rptPosts.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error loading posts.');</script>");
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
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fuImage.FileName);
                string savePath = Server.MapPath("~/Uploads/") + fileName;

                using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(fuImage.PostedFile.InputStream))
                {
                    using (Bitmap resizedImg = ResizeForInstagram(originalImage, 1080)) // Make it a 1080x1080 square
                    {
                        resizedImg.Save(savePath, ImageFormat.Jpeg);
                    }
                }

                imagePath = "Uploads/" + fileName;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                string query = "INSERT INTO Posts (UserID, Content, ImagePath, CreatedAt) VALUES (@UserID, @Content, @ImagePath, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@ImagePath", (object)imagePath ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            txtPost.Text = "";
            LoadPosts();
        }


        protected DataTable GetComments(int postId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                string query = @"
            SELECT c.CommentText, u.FullName
            FROM Comments c
            INNER JOIN Users u ON c.UserID = u.UserID
            WHERE c.PostID = @PostID
            ORDER BY c.CreatedAt ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PostID", postId);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Button btnComment = (Button)sender;
            int postID = Convert.ToInt32(btnComment.CommandArgument);

            RepeaterItem item = (RepeaterItem)btnComment.NamingContainer;
            TextBox txtComment = (TextBox)item.FindControl("txtComment");

            if (!string.IsNullOrWhiteSpace(txtComment.Text))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
                {
                    string query = "INSERT INTO Comments (PostID, UserID, CommentText, CreatedAt) VALUES (@PostID, @UserID, @CommentText, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", postID);
                        cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                        cmd.Parameters.AddWithValue("@CommentText", txtComment.Text.Trim());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                txtComment.Text = ""; // Clear the comment input

                // ✅ Redirect to the same page to prevent form resubmission
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }



        private Bitmap ResizeForInstagram(System.Drawing.Image img, int maxSize)
        {
            int originalWidth = img.Width;
            int originalHeight = img.Height;

            // Determine the scaling factor
            float scale = Math.Min((float)maxSize / originalWidth, (float)maxSize / originalHeight);
            int newWidth = (int)(originalWidth * scale);
            int newHeight = (int)(originalHeight * scale);

            // Create a square canvas
            Bitmap newImage = new Bitmap(maxSize, maxSize);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.Clear(Color.White); // Set white background
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(img, (maxSize - newWidth) / 2, (maxSize - newHeight) / 2, newWidth, newHeight);
            }

            return newImage;
        }


        // ✅ Helper function to resize images while maintaining aspect ratio
        private Bitmap ResizeImage(System.Drawing.Image img, int maxWidth, int maxHeight)
        {
            int newWidth = img.Width;
            int newHeight = img.Height;

            if (newWidth > maxWidth || newHeight > maxHeight)
            {
                float ratioX = (float)maxWidth / newWidth;
                float ratioY = (float)maxHeight / newHeight;
                float ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(newWidth * ratio);
                newHeight = (int)(newHeight * ratio);
            }

            Bitmap resizedImg = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, newWidth, newHeight);
            }

            return resizedImg;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
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

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                string query = "INSERT INTO Likes (UserID, PostID) SELECT @UserID, @PostID WHERE NOT EXISTS (SELECT 1 FROM Likes WHERE UserID = @UserID AND PostID = @PostID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                    cmd.Parameters.AddWithValue("@PostID", postID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadPosts();
        }
    }
}
