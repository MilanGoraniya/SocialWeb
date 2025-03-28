using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace SocialMediaPlatform
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "INSERT INTO Users (FullName, Email, Password) VALUES (@FullName, @Email, @Password)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Response.Write("<script>alert('Registration successful! Please login.');</script>");
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error during registration! Try again.');</script>");
                    }
                }
            }
        }
    }
}
