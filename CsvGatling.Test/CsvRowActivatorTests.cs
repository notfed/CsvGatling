using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGatling.Test
{
    [TestClass]
    public class CsvRowActivatorTests
    {
        private ICsvRowActivator<T> CreateSut<T>(string[] headerrow)
        {
            ICsvRowActivatorFactoryFactory<T> csvRowActivatorFactoryFactory
                = new DynamicCsvRowActivatorFactoryFactory<T>();
            IColumnNameMapper columnNameMapper = new DefaultColumnNameMapper();
            IConverterProvider converterProvider = new DefaultConverterProvider();
            ICsvRowActivatorFactory<T> csvRowActivatorFactory
                = csvRowActivatorFactoryFactory.Create(converterProvider, columnNameMapper);
            ICsvRowActivator<T> activator = csvRowActivatorFactory.Create(headerrow);
            return activator;
        }

        [TestMethod]
        public void CanParseSingleJ139Report()
        {
            // Arrange
            string[] headerrow = { "ClientId", "BirthDate", "Weight", "ReportName" };
            ICsvRowActivator<J139Report> workload = CreateSut<J139Report>(headerrow);
            string[] valuerow = { "156", "5/3/2013", "3.14", "J139" };

            // Act
            J139Report report = workload.CreateFromRow(valuerow);
            
            // Assert
            Assert.AreEqual(156, report.ClientId);
            Assert.AreEqual(new DateTime(2013,5,3), report.BirthDate);
            Assert.AreEqual(3.14, report.Weight);
            Assert.AreEqual("J139", report.ReportName);
        }
        [TestMethod]
        public void CanReorderFields()
        {
            // Arrange
            string[] headerrow = { "ReportName", "Weight", "BirthDate", "ClientId" };
            ICsvRowActivator<J139Report> workload = CreateSut<J139Report>(headerrow);

            string[] valuerow = { "J139", "3.14", "5/3/2013", "156" };

            // Act
            J139Report report = workload.CreateFromRow(valuerow);

            // Assert
            Assert.AreEqual(156, report.ClientId);
            Assert.AreEqual(new DateTime(2013, 5, 3), report.BirthDate);
            Assert.AreEqual(3.14, report.Weight);
            Assert.AreEqual("J139", report.ReportName);
        }
        [TestMethod]
        public void CanJustPullSingleField()
        {
            // Arrange
            string[] headerrow = { "Weight" };
            ICsvRowActivator<J139Report> workload = CreateSut<J139Report>(headerrow);

            string[] valuerow = { "3.14" };

            // Act
            J139Report report = workload.CreateFromRow(valuerow);

            // Assert
            Assert.AreEqual(0, report.ClientId);
            Assert.AreEqual(DateTime.MinValue, report.BirthDate);
            Assert.AreEqual(3.14, report.Weight);
            Assert.AreEqual(null, report.ReportName);
        }
        [TestMethod]
        public void PullsEmptyStringForEmpty()
        {
            // Arrange
            string[] headerrow = { "ReportName" };
            ICsvRowActivator<J139Report> workload = CreateSut<J139Report>(headerrow);

            string[] valuerow = { "" };

            // Act
            J139Report report = workload.CreateFromRow(valuerow);

            // Assert
            Assert.AreEqual("", report.ReportName);
        }
        [TestMethod]
        public void EmptyGoesToNullForNullableFields()
        {
            // Arrange
            string[] headerrow = { "ClientId", "Weight", "BirthDate" };
            ICsvRowActivator<J139ReportWithNullables> workload = CreateSut<J139ReportWithNullables>(headerrow);

            string[] valuerow = { "", "", ""};

            // Act
            J139ReportWithNullables report = workload.CreateFromRow(valuerow);

            // Assert
            Assert.AreEqual(null, report.ClientId);
            Assert.AreEqual(null, report.Weight);
            Assert.AreEqual(null, report.BirthDate);
        }
        [TestMethod]
        public void NullPropertiesForMissingFields()
        {
            // Arrange
            string[] headerrow = { "A", "B", "C" };
            ICsvRowActivator<J139ReportWithNullables> workload = CreateSut<J139ReportWithNullables>(headerrow);

            string[] valuerow = { "1", "3", "5/3/2013" };

            // Act
            J139ReportWithNullables report = workload.CreateFromRow(valuerow);

            // Assert
            Assert.AreEqual(null, report.ClientId);
            Assert.AreEqual(null, report.Weight);
            Assert.AreEqual(null, report.BirthDate);
        }
    }
}
