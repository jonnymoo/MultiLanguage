using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.C360.Language
{
    /// <summary>
    /// An interface for determining or changing the current language
    /// </summary>
    public interface ILanguage
    {
        /// <summary>
        /// Get the current language
        /// </summary>
        /// <returns></returns>
        string CurrentLanguage { get; set; }
    }
}
