using Civica.C360.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace LanguageModule
{
    public class HttpModule : IHttpModule, IRequiresSessionState
    {
        public void Dispose() { }
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += (sender, e) =>
            {
                var app = sender as HttpApplication;
                if (app != null)
                {
                    var requestContext = app.Context;
                    if (requestContext != null)
                    {
                        requestContext.Response.Filter = new ResponseStream(requestContext.Response.Filter, new Response(requestContext), new Translator(new Language(HttpContext.Current), new FileLanguagePackService()));
                    }
                }
            };

        }
    }
}
