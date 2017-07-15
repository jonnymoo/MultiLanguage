using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace LanguageModule
{
    public class HttpModule : IHttpModule
    {
        public void Dispose() { }
        public void Init(HttpApplication context)
        {
            context.PostRequestHandlerExecute += (sender, e) =>
            {
                var app = sender as HttpApplication;
                if (app != null)
                {
                    var requestContext = app.Context;
                    if(requestContext!=null && requestContext.Response.ContentType == "text/html")
                    {
                        //requestContext.Response.Filter = new ResponseStream(requestContext.Response.Filter);
                    }
                }

            };
        }
    }
}
