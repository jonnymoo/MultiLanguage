using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageModule
{
    public interface IResponse
    {
        bool IsText { get; }
        Encoding Encoding { get; }
    }
}
