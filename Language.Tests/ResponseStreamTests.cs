using Civica.C360.Language;
using LanguageModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Language.Tests
{
    [TestClass]
    public class ResponseStreamTests
    {
        [TestMethod]
        public void Given_a_stream_when_I_write_to_it_I_expect_the_get_same_results_written_in_my_underlying_stream()
        {
            var stream = new MemoryStream();

            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var response = new Mock<IResponse>();

            response.Setup(r => r.IsText).Returns(true);
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            var translator = new Translator(lang.Object, languagePacks.Object);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("A test");
            responseStream.Write(testBytes, 0, testBytes.Length);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(6, stream.Length);

            var output = new Byte[6];
            stream.Read(output, 0, 6);


            Assert.AreEqual("A test", Encoding.UTF8.GetString(output) );
        }

        [TestMethod]
        public void Given_a_stream_when_I_write_to_it_using_an_offset_I_expect_the_get_same_results_written_in_my_underlying_stream()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("A test");
            responseStream.Write(testBytes, 2, testBytes.Length-2);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 4);

            var output = new Byte[4];
            stream.Read(output, 0, 4);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "test");
        }


        [TestMethod]
        public void Given_a_stream_when_I_write_to_it_using_a_count_I_expect_the_get_same_results_written_in_my_underlying_stream()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);


            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("A test");
            responseStream.Write(testBytes, 0, testBytes.Length - 2);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 4);

            var output = new Byte[4];
            stream.Read(output, 0, 4);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "A te");
        }

        [TestMethod]
        public void Given_a_stream_when_I_have_a_language_replacement_I_expect_it_to_be_default_replaced()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);


            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %%Civica.Lang:TestArea.Some other stuff%% yet some more stuff");
            responseStream.Write(testBytes, 0, testBytes.Length);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 47);

            var output = new Byte[47];
            stream.Read(output, 0, 47);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff Some other stuff yet some more stuff");
        }

        [TestMethod]
        public void Given_a_stream_when_I_have_two_language_replacements_I_expect_them_to_be_default_replaced()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %%Civica.Lang:TestArea.Some other stuff%% yet some more stuff %%Civica.Lang:TestArea.SubArea.A bit more%%");
            responseStream.Write(testBytes, 0, testBytes.Length);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 58);

            var output = new Byte[58];
            stream.Read(output, 0, 58);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff Some other stuff yet some more stuff A bit more");
        }

        [TestMethod]
        public void Given_a_stream_when_I_have_an_unclosed_tag_I_do_not_expect_it_to_be_replaced()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %%Civica.Lang:TestArea.Some other stuff");
            responseStream.Write(testBytes, 0, testBytes.Length);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 50);

            var output = new Byte[50];
            stream.Read(output, 0, 50);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff %%Civica.Lang:TestArea.Some other stuff");
        }

        [TestMethod]
        public void Given_a_stream_when_my_key_is_split_over_two_writes_I_expect_it_to_be_replaced()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %%Civica.Lang:");
            responseStream.Write(testBytes, 0, testBytes.Length);

            testBytes = Encoding.UTF8.GetBytes("TestArea.Some other stuff%% yet some more stuff");
            responseStream.Write(testBytes, 0, testBytes.Length);

            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 47);

            var output = new Byte[47];
            stream.Read(output, 0, 47);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff Some other stuff yet some more stuff");
        }

        [TestMethod]
        public void Given_a_stream_when_my_tag_bit_of_the_key_is_split_over_two_writes_I_expect_it_to_be_replaced()
        {
            var stream = new MemoryStream();
            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %");
            responseStream.Write(testBytes, 0, testBytes.Length);

            testBytes = Encoding.UTF8.GetBytes("%Civica.Lang:TestArea.Some other stuff%% yet some more stuff");
            responseStream.Write(testBytes, 0, testBytes.Length);

            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 47);

            var output = new Byte[47];
            stream.Read(output, 0, 47);


            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff Some other stuff yet some more stuff");
        }

        [TestMethod]
        public void Given_a_stream_when_I_have_a_language_replacement_I_expect_it_to_be_translated_if_one_exists()
        {
            var stream = new MemoryStream();

            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            languagePacks.Setup(s => s.GetLanguagePack(It.IsAny<string>()))
                .Returns(new Dictionary<string, string>() { { "TestArea.Some other stuff", "translated" } });
 
            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(true);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %%Civica.Lang:TestArea.Some other stuff%% yet some more stuff");
            responseStream.Write(testBytes, 0, testBytes.Length);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 41);

            var output = new Byte[41];
            stream.Read(output, 0, 41);

            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff translated yet some more stuff");
        }

        [TestMethod]
        public void Given_a_stream_when_I_am_not_text_I_do_not_expect_to_attempt_translation()
        {
            var stream = new MemoryStream();

            var lang = new Mock<ILanguage>();
            var languagePacks = new Mock<ILanguagePackService>();
            languagePacks.Setup(s => s.GetLanguagePack(It.IsAny<string>()))
                .Returns(new Dictionary<string, string>() { { "TestArea.Some other stuff", "translated" } });

            var translator = new Translator(lang.Object, languagePacks.Object);
            var response = new Mock<IResponse>();
            response.Setup(r => r.Encoding).Returns(Encoding.UTF8);
            response.Setup(r => r.IsText).Returns(false);

            var responseStream = new ResponseStream(stream, response.Object, translator);
            var testBytes = Encoding.UTF8.GetBytes("Some stuff %%Civica.Lang:TestArea.Some other stuff%% yet some more stuff");
            responseStream.Write(testBytes, 0, testBytes.Length);
            responseStream.Flush();

            stream.Position = 0;
            Assert.AreEqual(stream.Length, 72);

            var output = new Byte[72];
            stream.Read(output, 0, 72);

            Assert.AreEqual(Encoding.UTF8.GetString(output), "Some stuff %%Civica.Lang:TestArea.Some other stuff%% yet some more stuff");
        }
    }
}
