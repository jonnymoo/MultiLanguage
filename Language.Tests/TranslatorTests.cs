using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Civica.C360.Language;
using Moq;
using System.Collections.Generic;

namespace Language.Tests
{
    [TestClass]
    public class TranslatorTests
    {
        [TestMethod]
        public void Given_no_language_packs_when_I_translate_a_word_I_expect_the_same_word_back()
        {
            var lang = new Mock<ILanguage>();
            var pack = new Mock<ILanguagePackService>();

            var service = new Translator(lang.Object, pack.Object);
            Assert.AreEqual("I am a test", service.Translate("I am a test"));
        }

        [TestMethod]
        public void Given_no_language_packs_when_I_translate_a_key_with_an_area_in_it_I_expect_only_the_last_section_back()
        {
            var lang = new Mock<ILanguage>();
            var pack = new Mock<ILanguagePackService>();

            var service = new Translator(lang.Object, pack.Object);
            Assert.AreEqual("I am a test", service.Translate("Area1.SubArea2.I am a test"));
        }

        [TestMethod]
        public void Given_an_english_language_pack_when_the_word_exists_I_expect_it_translated()
        {
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            languagePacks.Setup( s=> s.GetLanguagePack(It.IsAny<string>()))
                .Returns<string>( l =>
                {
                    if(l == "en")
                    {
                        return new Dictionary<string, string>() { { "I am a test", "translated" } };
                    }
                    else
                    {
                        return new Dictionary<string, string>();
                    }
                });

            var service = new Translator(lang.Object, languagePacks.Object);
            Assert.AreEqual("translated", service.Translate("I am a test"));

        }

        [TestMethod]
        public void Given_an_welsh_language_pack_when_the_word_exists_I_expect_it_translated()
        {
            var lang = new Mock<ILanguage>();
            lang.Setup(l => l.GetCurrentLanguage())
                .Returns("cy");

            var languagePacks = new Mock<ILanguagePackService>();
            languagePacks.Setup(s => s.GetLanguagePack(It.IsAny<string>()))
                .Returns<string>(l =>
                {
                    if (l == "cy")
                    {
                        return new Dictionary<string, string>() { { "I am a test", "translated" } };
                    }
                    else
                    {
                        return new Dictionary<string, string>();
                    }
                });

            var service = new Translator(lang.Object, languagePacks.Object);
            Assert.AreEqual("translated", service.Translate("I am a test"));

        }
    }
}
