using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CsvGatling.Test
{
    [TestClass]
    public class CsvReaderIntegratedPerformanceTests
    {
        private int rowcount = 100000;
        private string testCsvFilePath = @"C:\Temp\SampleTestCsvGatlingFilePerformanceTest.csv";

        [TestMethod]
        public void CreateTestFile()
        {
            Fixture fixture = new Fixture();
            J139Report testReport = fixture.CreateAnonymous<J139Report>();

            using (StreamWriter writer = new StreamWriter(testCsvFilePath, false, Encoding.UTF8, 4096))
            {
                writer.WriteLine("ClientId,BirthDate,Weight,ReportName");
                for (int i = 0; i < rowcount; i++)
                {
                    writer.WriteLine(string.Format("{0},{1},{2},{3}",
                        testReport.ClientId,
                        testReport.BirthDate.ToString("MM/dd/yy"),
                        testReport.Weight,
                        testReport.ReportName)
                    );
                }
                writer.Flush();
            }
        }

        private CsvReader<T> CreateSut<T>(Stream fileStream, int count)
        {
            return new CsvReader<T>(fileStream);
        }

        [TestMethod]
        public void PerformanceTestInitializations()
        {
            // Arrange
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Act
            using (FileStream fileStream = new FileStream(testCsvFilePath, FileMode.Open))
            {
                CsvReader<J139Report> sut = CreateSut<J139Report>(fileStream, rowcount);
            }

            // Assert
            TimeSpan endTime = stopwatch.Elapsed;
            Console.Write("Initialization took " + endTime.TotalMilliseconds + "ms");
        }
        [TestMethod]
        public void IntegratedPerformanceTestBulk()
        {
            // Arrange
            if (!File.Exists(testCsvFilePath))
            {
                CreateTestFile();
            }
            Stopwatch stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();

            int total = 0;
            using (Stream fileStream = new FileStream(testCsvFilePath, FileMode.Open))
            {
                CsvReader<J139Report> sut = CreateSut<J139Report>(fileStream, rowcount);
                IEnumerable<J139Report> reports = sut.ReadAll();
                foreach (J139Report report in reports)
                {
                    total++;
                }
            }

            stopwatch.Stop();
            TimeSpan endTime = stopwatch.Elapsed;

            // Output
            Console.Write(total + " rows took " + endTime.TotalMilliseconds + "ms");
        }
        [TestMethod]
        public void NormalFileReadPerformanceTestBulk()
        {
            // Arrange
            if (!File.Exists(testCsvFilePath))
            {
                CreateTestFile();
            }
            Stopwatch stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();

            int total = 0;
            using (FileStream fileStream = new FileStream(testCsvFilePath, FileMode.Open))
            {
                StreamReader reader = new StreamReader(fileStream);
                while (!reader.EndOfStream)
                {
                    reader.ReadLine();
                    total++;
                }
            }
            TimeSpan endTime = stopwatch.Elapsed;

            // Output
            Console.Write(total + " lines took " + endTime.TotalMilliseconds + "ms");
        }
    }
}
