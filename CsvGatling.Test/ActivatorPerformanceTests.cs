using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGatling.Test
{
    [TestClass]
    public class ActivatorPerformanceTests
    {
        private ICsvRowActivator<T> CreateSut<T>(string[] headerrow)
        {
            DynamicCsvRowActivatorFactoryFactory<T> csvRowActivatorFactoryFactory
                = new DynamicCsvRowActivatorFactoryFactory<T>();
            IColumnNameMapper columnNameMapper = new DefaultColumnNameMapper();
            IConverterProvider converterProvider = new DefaultConverterProvider();
            ICsvRowActivatorFactory<T> csvRowActivatorFactory =
                csvRowActivatorFactoryFactory.Create(converterProvider, columnNameMapper);
            ICsvRowActivator<T> activator = csvRowActivatorFactory.Create(headerrow);
            return activator;
        }


        [TestMethod]
        public void PerformanceTestInitializations()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Arrange
            string[] headerrow = { "ClientId", "BirthDate", "Weight", "ReportName" };
            ICsvRowActivator<J139Report> workload = CreateSut<J139Report>(headerrow);

            TimeSpan endTime = stopwatch.Elapsed;
            Console.Write("Initialization took " + endTime.Milliseconds + "ms");
        }
        [TestMethod]
        public void PerformanceTestBulk()
        {
            // Arrange
            string[] headerrow = { "ClientId", "BirthDate", "Weight", "ReportName" };
            ICsvRowActivator<J139Report> workload = CreateSut<J139Report>(headerrow);
            string[] valuerow = { "156", "5/3/2013", "3.14", "J139" };

            // Act
            
            int max = 1000000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < max; i++)
            {
                J139Report report = workload.CreateFromRow(valuerow);
            }
            TimeSpan endTime = stopwatch.Elapsed;
            Console.Write(max + " runs took " + endTime.Milliseconds + "ms");
        }
    }
}
