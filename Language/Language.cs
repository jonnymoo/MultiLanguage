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

        public void ChangeLanguage(string language)
        {
            var langCookie = new HttpCookie("Civica.Lang");
            langCookie.Value = "cy-GB";
            langCookie.HttpOnly = true;
            langCookie.Expires = DateTime.MinValue;
            langCookie.Value = language;
            current.Response.Cookies.Add(langCookie);
        }

        public string GetCurrentLanguage()
        {
            if (current != null && current.Response.Cookies != null && current.Response.Cookies["Civica.Lang"] != null && current.Response.Cookies["Civica.Lang"].Value != null)
            {
                return current.Response.Cookies["Civica.Lang"].Value;
            }
            else if (current!=null && current.Request.Cookies!=null && current.Request.Cookies["Civica.Lang"]!=null && current.Request.Cookies["Civica.Lang"].Value != null)
            {
                return current.Request.Cookies["Civica.Lang"].Value;
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
