using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageModule
{
    public class ResponseStream : Stream
    {
        private Stream _stream;

        public ResponseStream(Stream originalFilter)
        {
            _stream = originalFilter;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {

            string Content = "";

            for (int i = 0; i < count; i++)
            {
                Content = Content + (Char)buffer[i];
            }

            Content = Content.Replace("QWERTY", "BERTY");

            byte[] ContentArr = new byte[Content.Length];

            for (int i = 0; i < Content.Length; i++)
            {
                ContentArr[i] = (byte)Content[i];
            }

            _stream.Write(ContentArr, 0, Content.Length);

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long length)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { throw new NotSupportedException(); }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override void Close()
        {
            _stream.Close();
        }

    }
}
