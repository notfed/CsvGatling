using System;
using System.IO;
using System.Text;

namespace TestHelpers
{
    /// <summary>
    /// Streams out an infinite number lines of text 
    /// </summary>
    public class InfiniteCsvFile : Stream
    {
        private readonly byte[] headerline;
        private readonly byte[] valueline;

        public InfiniteCsvFile(string headerline, string valueline)
        {
            this.headerline = Encoding.ASCII.GetBytes(headerline);
            this.valueline = Encoding.ASCII.GetBytes(valueline);
        }
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        private long position = 0;
        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        int total = 0;
        public override int Read(byte[] buffer, int offset, int count)
        {
            int written = 0;
            // If we still haven't given out header.Length bytes, finish writing header
            if (total < headerline.Length)
            {
                int numberCharsLeftInHeader = (headerline.Length - total);
                // If we have enough space in the buffer to finish the header, write it all and continue
                if (count > numberCharsLeftInHeader)
                {
                    Buffer.BlockCopy(headerline, total, buffer, offset, numberCharsLeftInHeader);
                    total += numberCharsLeftInHeader;
                    written += numberCharsLeftInHeader;
                    offset += numberCharsLeftInHeader;
                    count -= numberCharsLeftInHeader;
                }
                // If we had exactly the right amount, write it and return
                // If we don't have enough space in the buffer, write as much as we can and return
                else if (count >= numberCharsLeftInHeader)
                {
                    int amountWeCanWrite = count - numberCharsLeftInHeader;
                    Buffer.BlockCopy(headerline, total, buffer, offset, amountWeCanWrite);
                    total += amountWeCanWrite;
                    written += amountWeCanWrite;
                    offset += amountWeCanWrite;
                    return written;
                }
            }

            // While we still have space left in the buffer, return a line
            while (count > 0)
            {
                int amountForRestOfLine = (written - headerline.Length) % valueline.Length;
                if (amountForRestOfLine == 0)
                {
                    amountForRestOfLine = valueline.Length;
                }

                // If we have enough for a full line, write it and continue
                if (count >= amountForRestOfLine)
                {
                    Buffer.BlockCopy(valueline, 0, buffer, offset, amountForRestOfLine);
                    total += amountForRestOfLine;
                    count -= amountForRestOfLine;
                    written += amountForRestOfLine;
                    offset += amountForRestOfLine;
                }
                // If we don't have room, write just enough and return
                else
                {
                    Buffer.BlockCopy(valueline, 0, buffer, offset, count);
                    written += count;
                    count = 0;
                }
            }
            return written;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
