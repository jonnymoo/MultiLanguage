using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.C360.Language
{
    public interface ILanguage
    {
        string GetCurrentLanguage();
        void ChangeLanguage(string language);

    }
}
