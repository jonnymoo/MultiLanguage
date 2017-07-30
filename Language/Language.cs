using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Civica.C360.Language
{

    /// <summary>
    /// Implementation of lanuague which stored the chosen language in a cookie
    /// </summary>
    public class Language : ILanguage
    {
        private HttpContext current;
       
        public Language(HttpContext current)
        {
            this.current = current;
        }

        /// <summary>
        /// Get language from cookie (and set them back into a cookie) - if not set then fall back to the current culture
        /// </summary>
        public string CurrentLanguage
        {
            get
            {
                // On response?
                if (current != null && current.Response.Cookies != null && current.Response.Cookies["Civica.Lang"] != null && current.Response.Cookies["Civica.Lang"].Value != null)
                {
                    return current.Response.Cookies["Civica.Lang"].Value;
                }
                // Request?
                else if (current != null && current.Request.Cookies != null && current.Request.Cookies["Civica.Lang"] != null && current.Request.Cookies["Civica.Lang"].Value != null)
                {
                    return current.Request.Cookies["Civica.Lang"].Value;
                }
                else
                {
                    // Current culture?
                    string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();

                    if (string.IsNullOrEmpty(lang))
                    {
                        //Default
                        lang = "en";
                    }

                    return lang;
                }
            }
            set
            {
                // Set a cookie on the response
                var langCookie = new HttpCookie("Civica.Lang");
                langCookie.HttpOnly = true;
                langCookie.Expires = DateTime.MinValue;
                langCookie.Value = value;
                current.Response.Cookies.Add(langCookie);
            }
        }
    }
}