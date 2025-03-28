using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SocialMediaPlatform
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblName.Text = "John Doe";
                lblEmail.Text = "john@example.com";
                imgProfile.ImageUrl = "Images/user.png";
            }
        }
    }
}
