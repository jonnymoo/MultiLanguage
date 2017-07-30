using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LanguageModule
{
    public class Response : IResponse
    {
        private HttpContext requestContext;

        public Response(HttpContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public bool IsText { get => requestContext.Response.ContentType == "text/html"; }
        public Encoding Encoding { get => requestContext.Response.ContentEncoding; }

        static int idebug = 1;

        public void debug()
        {
            string debug = "ContentType: " + requestContext.Response.ContentType + "\r\n";

            foreach (var key in requestContext.Response.Headers.AllKeys)
            {
                debug += key + ":" + requestContext.Response.Headers[key] + "\r\n";

            }
            File.WriteAllText("c:\\tmp\\debug"+(idebug++).ToString()+".txt", debug);

        }
    }
}
