using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace SocialMediaPlatform
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT UserID FROM Users WHERE Email=@Email AND Password=@Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        Session["UserID"] = result.ToString();
                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid credentials! Try again.');</script>");
                    }
                }
            }
        }
    }
}
