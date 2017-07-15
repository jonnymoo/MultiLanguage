using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Civica.C360.Language
{
    public class FileLanguagePackService : ILanguagePackService
    {
        static Dictionary<string, Dictionary<string, string>> files = new Dictionary<string, Dictionary<string, string>>();
        static Dictionary<string, string> empty = new Dictionary<string, string>();
        static FileLanguagePackService() {
            foreach (string file in Directory.EnumerateFiles(HostingEnvironment.MapPath("~/App_GlobalResources"), "*.lang"))
            {
                // Create all lines into a dictionary
                var lines = File.ReadAllLines(file);

                // The file name is the language string e.g. cy.lang
                files.Add(Path.GetFileName(file).Split('.')[0], lines.ToDictionary(x => x.Split('=')[0].Trim(), x => x.Split('=')[1].Trim()));
            }
        }
        public Dictionary<string, string> GetLanguagePack(string language)
        {
            if (files.ContainsKey(language))
            {
                return files[language];
            }
            else
            {
                return empty;
            }
        }
    }
}
