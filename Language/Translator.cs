using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Civica.C360.Language
{
    public class Translator
    {
        private ILanguage Lang;
        private ILanguagePackService LangaugePackService;

        /// <summary>
        /// Creates a new instance of the <see cref="Traslator"/> class
        /// </summary>
        /// <param name="lang"></param>
        public Translator(ILanguage lang, ILanguagePackService languagePackService)
        {
            this.Lang = lang;
            this.LangaugePackService = languagePackService;
        }
        
        
        /// <summary>
        /// Translate a phrase into the current language
        /// </summary>
        /// <param name="area"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public string Translate(string area, string from)
        {

            // Check if the word is in the languages langauge pack

            var pack = LangaugePackService.GetLanguagePack(Lang.GetCurrentLanguage());

            if(pack != null && pack.ContainsKey(from))
            {
                return pack[from];
            }


            // Is it in the english one?
            var english = LangaugePackService.GetLanguagePack("en");

            if (english != null && english.ContainsKey(from))
            {
                return english[from];
            }

            return from;
        }
    }
}