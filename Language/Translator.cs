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
        /// Creates a new instance of the <see cref="Translator"/> class
        /// </summary>
        /// <param name="lang"></param>
        public Translator(ILanguage lang, ILanguagePackService languagePackService)
        {
            this.Lang = lang;
            this.LangaugePackService = languagePackService;
        }
        
        
        /// <summary>
        /// Translate a key into the current language
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Translate(string key)
        {

            // Check if the word is in the languages langauge pack

            var pack = LangaugePackService.GetLanguagePack(Lang.CurrentLanguage);

            if(pack != null && pack.ContainsKey(key))
            {
                return pack[key];
            }


            // Is it in the english one?
            var english = LangaugePackService.GetLanguagePack("en");

            if (english != null && english.ContainsKey(key))
            {
                return english[key];
            }

            // The default translation is the last section of the key
            return key.Substring(key.LastIndexOf(".") + 1);
        }
    }
}