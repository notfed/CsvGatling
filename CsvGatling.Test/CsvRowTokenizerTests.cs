using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelpers;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CsvGatling.Test
{
    [TestClass]
    public class CsvRowTokenizerTests
    {
        [TestMethod]
        public void SplitsApartSimpleLine()
        {
            string fakeCsvRow = "1,2,3";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);
            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
        }

        [TestMethod]
        public void SplitsApartValuelessLine()
        {
            string fakeCsvRow = ",,";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);
            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual("", tokens[0]);
            Assert.AreEqual("", tokens[1]);
            Assert.AreEqual("", tokens[2]);
        }
        [TestMethod]
        public void SplitsApartLineWithDoubleQuotedValue()
        {
            string fakeCsvRow = "1,\"2\",3";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
        }
        [TestMethod]
        public void SplitsApartLineWithQuotesWithCommas()
        {
            string fakeCsvRow = "\"1,2,3\"";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual("1,2,3", tokens[0]);
        }
        [TestMethod]
        public void SplitsApartLineWithBlankField()
        {
            string fakeCsvRow = "\"\",1,2,3";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("", tokens[0]);
            Assert.AreEqual("1", tokens[1]);
            Assert.AreEqual("2", tokens[2]);
            Assert.AreEqual("3", tokens[3]);
        }
        [TestMethod]
        public void SplitsApartQuoteAcrossWindowsLines()
        {
            string fakeCsvRow = "1,\"2\r\n3\",4";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2\r\n3", tokens[1]);
            Assert.AreEqual("4", tokens[2]);
        }
        [TestMethod]
        public void SplitsApartQuoteAcrossUnixLines()
        {
            string fakeCsvRow = "1,\"2\n3\",4";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2\n3", tokens[1]);
            Assert.AreEqual("4", tokens[2]);
        }
        [TestMethod]
        public void AllowsBigRowsAndQuotesWithinQuotes()
        {
            string fakeCsvRow = "1,\"My \"\"\"\"name\"\"\"\" is \"\"X\"\"\",4";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("My \"\"name\"\" is \"X\"", tokens[1]);
            Assert.AreEqual("4", tokens[2]);
        }
        [TestMethod]
        public void AllowsBigRowsAndQuotesWithinQuotesWithManyChunks()
        {
            string fakeCsvRow = "1,\"My \"\"\"\"name\"\"\"\" is \"\"X\"\"\",4";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);
            Stream chunkedStream = new SerialStreamAdapter(csvStream, 2);

            ICsvRowTokenizer sut = new CsvRowTokenizer(chunkedStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("My \"\"name\"\" is \"X\"", tokens[1]);
            Assert.AreEqual("4", tokens[2]);
        }
        [TestMethod]
        public void CanReadMultipleLines()
        {
            string fakeCsvRow = "1\r\n2\r\n3\r\n";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[][] rows = sut.ReadAllRows().ToArray();

            Assert.AreEqual(3, rows.Length);
            Assert.AreEqual("1", rows[0].Single());
            Assert.AreEqual("2", rows[1].Single());
            Assert.AreEqual("3", rows[2].Single());
        }
        [TestMethod]
        public void CanReadMultipleLinesWithNoLastNewline()
        {
            string fakeCsvRow = "1\r\n2\r\n3";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[][] rows = sut.ReadAllRows().ToArray();

            Assert.AreEqual(3, rows.Length);
            Assert.AreEqual("1", rows[0].Single());
            Assert.AreEqual("2", rows[1].Single());
            Assert.AreEqual("3", rows[2].Single());
        }
        [TestMethod]
        public void CanReadMultipleLinesWithChunkedStream()
        {
            string fakeCsvRow = "1\r\n2\r\n3";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);
            Stream chunkedStream = new SerialStreamAdapter(csvStream, 1);

            ICsvRowTokenizer sut = new CsvRowTokenizer(chunkedStream);
            string[][] rows = sut.ReadAllRows().ToArray();

            Assert.AreEqual(3, rows.Length);
            Assert.AreEqual("1", rows[0].Single());
            Assert.AreEqual("2", rows[1].Single());
            Assert.AreEqual("3", rows[2].Single());
        }


        [TestMethod]
        public void AllowsMissingLastNewline()
        {
            string fakeCsvRow = "1,2,3";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
        }
        [TestMethod]
        public void AllowsLastCharQuote()
        {
            string fakeCsvRow = "1,2,\"3\"";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
        }
        [TestMethod]
        public void AllowsLastCharEscapedQuote()
        {
            string fakeCsvRow = "1,2,3\"\"\"\"";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3\"", tokens[2]);
        }
        [TestMethod]
        public void AllowsMissingLastNewlineWithLastCharQuote()
        {
            string fakeCsvRow = "1,2,\"3\"";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
        }
        [TestMethod]
        public void AllowsMissingLastNewlineWithLastCharQuoteWithManyChunks()
        {
            string fakeCsvRow = "1,2,\"3\"";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);
            Stream manyChunksStream = new SerialStreamAdapter(csvStream, 1);

            ICsvRowTokenizer sut = new CsvRowTokenizer(manyChunksStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
        }
        [TestMethod]
        public void AllowsMissingLastNewlineWithLastCharCarriageReturn()
        {
            string fakeCsvRow = "1,2,3\r";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3\r", tokens[2]);
        }
        [TestMethod]
        public void AllowsMissingLastNewlineWithLastCharComma()
        {
            string fakeCsvRow = "1,2,";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("", tokens[2]);
        }

        [TestMethod]
        public void AllowsQuotesWithinQuotesWithManyChunks2()
        {
            string fakeCsvRow = ",\"\"\"\"\"\",";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);
            Stream serialStream = new SerialStreamAdapter(csvStream, 1);

            ICsvRowTokenizer sut = new CsvRowTokenizer(serialStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("", tokens[0]);
            Assert.AreEqual("\"\"", tokens[1]);
            Assert.AreEqual("", tokens[2]);
        }
        [TestMethod]
        public void AllowsQuotesWithinQuotes3()
        {
            string fakeCsvRow = "\"\"\"\"\"\"";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual("\"\"", tokens[0]);
        }
        [TestMethod]
        public void CanReadWithManyChunks()
        {
            string fakeCsvRow = "\"\"\"\"\"\"";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);
            Stream manyChunksStream = new SerialStreamAdapter(csvStream, 1);

            ICsvRowTokenizer sut = new CsvRowTokenizer(manyChunksStream);
            string[] tokens = sut.ReadAllRows().First();

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual("\"\"", tokens[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void IncompleteQuoteThrowsError()
        {
            string fakeCsvRow = "\"hi how are y";

            Stream csvStream = CreateMemoryStreamNoLastNewline(fakeCsvRow);

            ICsvRowTokenizer sut = new CsvRowTokenizer(csvStream);
            string[] tokens = sut.ReadAllRows().First();
        }

        private Stream CreateMemoryStream(params string[] lines)
        {
            MemoryStream csvStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(csvStream);
            foreach (var line in lines)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Flush();
            csvStream.Position = 0;
            return csvStream;
        }
        private Stream CreateMemoryStreamNoLastNewline(params string[] lines)
        {
            MemoryStream csvStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(csvStream);
            int i = 0;
            int maxindex = lines.Count()-1;
            foreach (var line in lines)
            {
                if (i == maxindex)
                {
                    streamWriter.Write(line);
                }
                else
                {
                    streamWriter.WriteLine(line);
                }
            }
            streamWriter.Flush();
            csvStream.Position = 0;
            return csvStream;
        }
    }
}
