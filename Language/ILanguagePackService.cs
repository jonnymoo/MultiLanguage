using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.C360.Language
{
    public interface ILanguagePackService
    {
        Dictionary<string, string> GetLanguagePack(string language);    
    }
}
