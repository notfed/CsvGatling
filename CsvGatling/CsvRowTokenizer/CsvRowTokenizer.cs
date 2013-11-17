using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CsvGatling
{
    /// <summary>
    /// Reads a complete CSV row (according to RFC 4180) from a stream, returning a list of columns.
    /// A new row is delimited by either \r\n or \n, and quoted fields use double quotes.
    /// </summary>
    public class CsvRowTokenizer : ICsvRowTokenizer
    {
        private readonly char quoteCharacter = '"';
        private readonly Stream csvStream;
        private readonly char columnDelimiter;

        public CsvRowTokenizer(Stream csvStream, char columnDelimiter = ',')
        {
            this.csvStream = csvStream;
            this.columnDelimiter = columnDelimiter;
        }

        public IEnumerable<string[]> ReadAllRows()
        {
            StreamReader csvStreamReader = new StreamReader(csvStream);
            bool endOfStreamReached = false;
            char[] tempBuffer = new char[4096];
            StringBuilder buffer = new StringBuilder(4096);

            // Continuously read from stream until end of file is reached and all data has been returned
            while (!endOfStreamReached)
            {
                // Read some bytes into buffer
                int readCount = csvStreamReader.Read(tempBuffer, 0, 4096);
                if (readCount <= 0)
                {
                    endOfStreamReached = true;
                }
                else
                {
                    // Append the chunk to the unbounded buffer
                    buffer.Append(tempBuffer, 0, readCount);
                }

                // Return as many rows as possible from buffer
                string[] rowChunks;
                int rowLength;

                // Keep trying to tokenize a complete row
                while (TryTokenize(buffer, out rowChunks, out rowLength, endOfStreamReached))
                {
                    // Return the CSV row
                    yield return rowChunks;

                    // Strip the returned row from the buffer
                    buffer.Remove(0, rowLength);
                }
            }
        }

        /// <summary>
        /// Try to pull an entire row from the buffer and return it; return false if a row doesn't exist yet
        /// </summary>
        private bool TryTokenize(StringBuilder buffer, out string[] rowChunks, out int rowLength, bool reachedEndOfStream)
        {
            List<string> columns = new List<string>();
            StringBuilder currentToken = new StringBuilder();
            int bufferLength = buffer.Length;
            char previousChar = 'x';
            char currentChar = 'x';
            bool previousCharWasCandidateEndOfQuote = false;
            bool insideQuotes = false;

            for (int i = 0; i < bufferLength; i++)
            {
                previousChar = currentChar;
                currentChar = buffer[i];

                // If previous character was a candidate end of quote, and we see another quote,
                // then we've officially hit an end of quote
                if (insideQuotes && previousCharWasCandidateEndOfQuote && currentChar != quoteCharacter)
                {
                    previousCharWasCandidateEndOfQuote = false;
                    insideQuotes = false;
                }
                if (insideQuotes)
                {
                    // Unless we see a quote, keep appending to current token
                    if (currentChar != quoteCharacter)
                    {
                        currentToken.Append(currentChar);
                    }
                    // If we're inside quotes, and we see another quote, several things may happen
                    else
                    {
                        // If this is a quote-inside-a-quote, it might be an end of quote
                        if (!previousCharWasCandidateEndOfQuote)
                        {
                            previousCharWasCandidateEndOfQuote = true;
                        }
                        // Two quotes in a row is an escaped quote
                        else
                        {
                            currentToken.Append(quoteCharacter);
                            previousCharWasCandidateEndOfQuote = false;
                        }
                    }
                }
                else
                {

                    // If we're not in quotes, and we see a comma, we're done with this token and can move to the next
                    if (currentChar == columnDelimiter)
                    {
                        columns.Add(currentToken.ToString());
                        currentToken.Clear();
                    }
                    // If we're not inside quotes, and we see a quote mark, start quotes
                    else if (currentChar == quoteCharacter)
                    {
                        insideQuotes = true;
                    }
                    // If we're not in quotes, and we see a carriage return, skip it
                    else if (currentChar == '\r')
                    {
                    }
                    // If we're not in quotes, and the previous char was a carriage return, 
                    // and we DON'T see a line feed, then include the carriage return
                    else if (previousChar == '\r' && currentChar != '\n')
                    {
                        currentToken.Append('\r');
                        currentToken.Append(currentChar);
                    }
                    //  If we're not in quotes, and current character is a line feed, we've successfully parsed a complete row
                    else if (currentChar == '\n')
                    {
                        columns.Add(currentToken.ToString());
                        rowChunks = columns.ToArray();
                        rowLength = i + 1;
                        return true;
                    }
                    else
                    {
                        // Current character is a normal character, add it to the token
                        currentToken.Append(currentChar);
                    }
                }

                // If we're at end of buffer, and end of stream, then we're done
                if (i + 1 == bufferLength && reachedEndOfStream)
                {
                    // But if we're inside quotes still, that's a problem
                    if (insideQuotes && !previousCharWasCandidateEndOfQuote)
                    {
                        throw new InvalidDataException("Reached end of file before reaching end quote!");
                    }
                    // If there was a carriage return without a newline, include the carriage return in results
                    if (currentChar == '\r')
                    {
                        currentToken.Append('\r');
                    }
                    columns.Add(currentToken.ToString());
                    rowChunks = columns.ToArray();
                    rowLength = i + 1;
                    return true;
                }
            }

            // If we made it here, we hit all the characters without completing a row
            rowChunks = null;
            rowLength = 0;
            return false;
        }
    }
}
