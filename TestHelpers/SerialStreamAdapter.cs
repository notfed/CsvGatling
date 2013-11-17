using System;
using System.IO;
using System.Text;

namespace TestHelpers
{
    /// <summary>
    /// Adapts a stream, returning chunks of a given maximum size
    /// </summary>
    public class SerialStreamAdapter : Stream
    {
        private readonly Stream inStream;
        private readonly int chunkSize;

        public SerialStreamAdapter(Stream inStream, int chunkSize)
        {
            if (chunkSize < 1)
            {
                throw new ArgumentOutOfRangeException("chunkSize");
            }
            this.inStream = inStream;
            this.chunkSize = chunkSize;
        }
        public override bool CanRead
        {
            get
            {
                return inStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get { return inStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return inStream.CanWrite; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                return inStream.Position;
            }
            set
            {
                inStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return inStream.Read(buffer, offset, Math.Min(chunkSize, count));
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return inStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            inStream.Write(buffer, offset, count);
        }
    }
}
