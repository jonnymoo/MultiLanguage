using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Civica.C360.Language
{
    public class Language : ILanguage
    {
        private HttpContext current;
       
        public Language(HttpContext current)
        {
            this.current = current;
        }

        public string GetCurrentLanguage()
        {
            if(current!=null && current.Session!=null && current.Session["lang"]!=null)
            {
                return current.Session["lang"].ToString();
            }
            else
            {
                string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();

                if(string.IsNullOrEmpty(lang))
                {
                    lang = "en";
                }

                return lang;
            }
        }
    }
}
