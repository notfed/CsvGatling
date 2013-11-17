using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CsvGatling.Test
{
    [TestClass]
    public class CsvReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CsvReaderThrowsOnColumnCountMismatch()
        {
            string fakeCsvRow = "1,2,3\r\na,b,c\r\none,two,three\r\nRED";

            Stream csvStream = CreateMemoryStream(fakeCsvRow);
            CsvReader<J139Report> sut = new CsvReader<J139Report>(csvStream);

            var result = sut.ReadAll().ToList();

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
    }
}
