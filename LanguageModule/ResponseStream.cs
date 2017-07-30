using Civica.C360.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LanguageModule
{
    /// <summary>
    /// Response stream decorator which can translate message keys as it writes
    /// </summary>
    public class ResponseStream : Stream
    {
        const string START_TAG = "%%Civica.Lang:";
        const int START_TAG_LEN = 14;
        const string END_TAG = "%%";
        const int END_TAG_LEN = 2;

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream Stream;

        /// <summary>
        /// The response
        /// </summary>
        private IResponse Response;

        /// <summary>
        /// Used to hold onto a retained bit of the last call to write if we thing it may contain a start tag and the key spans writes
        /// </summary>
        private string Retained = string.Empty;

        /// <summary>
        /// Used to do the actual translation
        /// </summary>
        private Translator Translator;

        /// <summary>
        /// Creates a new instance of the <see cref="ResponseStream"/> class
        /// </summary>
        /// <param name="originalFilter">The underlying stream</param>
        /// <param name="encoding">The encoding of the text content</param>
        public ResponseStream(Stream originalFilter, IResponse response, Translator translator)
        {
            this.Stream = originalFilter;
            this.Response = response;
            this.Translator = translator;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
 
            if (!this.Response.IsText)
            {
                this.Stream.Write(buffer, offset, count);
                return;
            }

 
            string requestBuffer = this.Retained + Response.Encoding.GetString(buffer,offset,count);

            this.Retained = string.Empty;

            int start = 0;
            // Does the request buffer contain the Start Tag ?
            int ind = requestBuffer.IndexOf(START_TAG);

            StringBuilder ret = new StringBuilder();

            while(ind >=0)
            {
                // Get where the key starts
                int keyStart = ind + START_TAG_LEN;

                // Get where the key ends
                int keyEnd = requestBuffer.IndexOf(END_TAG, keyStart);

                // No key end?  Ok - well break out of the loop - it may turn up later
                if (keyEnd == -1)
                {
                    break;
                }

                // Extract the key
                string key = requestBuffer.Substring(keyStart, keyEnd - keyStart);

                string translation = this.Translator.Translate(key);
                
                

                // Glue it back together - up to the where the tag started
                ret.Append(requestBuffer.Substring(start, ind - start));

                // The translation
                ret.Append(translation);

                // Our new start is now the end of the end tag...
                start = keyEnd + END_TAG_LEN;

                // Do we have another tag?
                ind = requestBuffer.IndexOf(START_TAG, start);
            }

            // If we don't think we have part of a tag to follow - append from the last starting point

            // To work this out - if ind doesn't == -1 - we must have had a start tag (because we have been in the while loop)
            bool needToRetain = true;
            string endBit = requestBuffer.Substring(start);
            
            if(ind == -1)
            {
                // May not need it - just need to check that none of the ends of the tag exist (otherwise it is a possibility)
                needToRetain = false;
                
                for (int i=0;i<START_TAG_LEN;i++)
                {
                    if (endBit.EndsWith(START_TAG.Substring(0, i+1)))
                    {
                        needToRetain = true;
                        break;
                    }
                }

            }

            if (needToRetain)
            {
                // Hold onto it for next time
                this.Retained = requestBuffer.Substring(start);
            }
            else
            {
                ret.Append(requestBuffer.Substring(start));
            }


            var response = Response.Encoding.GetBytes(ret.ToString());
            this.Stream.Write(response, 0,response.Length);

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.Stream.Read(buffer, offset, count);
        }

        public override void SetLength(long length)
        {
            this.Stream.SetLength(length);
        }

        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return this.Stream.Seek(offset, direction);
        }

        /// <summary>
        /// Make sure we have nothing left in buffers
        /// </summary>
        public override void Flush()
        {
            // We may have some stuff left in the retained string 
            var response = Response.Encoding.GetBytes(this.Retained.ToString());
            this.Stream.Write(response, 0, response.Length);

            this.Stream.Flush();
        }

        public override long Position
        {
            get { return this.Stream.Position; }
            set { this.Stream.Position = value; }
        }

        public override long Length
        {
            get { return this.Stream.Length; }
        }

        public override bool CanWrite
        {
            get { return this.Stream.CanWrite; }
        }

        public override bool CanSeek
        {
            get { return this.Stream.CanSeek; }
        }

        public override bool CanRead
        {
            get { return this.Stream.CanRead; }
        }

        public override void Close()
        {
            this.Stream.Close();
        }

    }
}
