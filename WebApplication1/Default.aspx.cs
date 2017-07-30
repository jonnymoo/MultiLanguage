using Civica.C360.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       
        protected void Welsh_Click(object sender, EventArgs e)
        {
            new Language(HttpContext.Current).CurrentLanguage = "cy-GB";
        }

        protected void English_Click(object sender, EventArgs e)
        {
            new Language(HttpContext.Current).CurrentLanguage = "en-GB";
        }
    }
}