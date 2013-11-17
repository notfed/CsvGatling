using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvGatling.Test
{
    [TestClass]
    public class CachingDynamicCsvRowActivatorFactoryTests
    {
        [TestMethod]
        public void PullsFromSourceFirstHit()
        {
            // Arrange
            Mock<IColumnNameMapper> mockColumnNameMapper = new Mock<IColumnNameMapper>();
            Mock<IConverterProvider> mockConverterProvider  = new Mock<IConverterProvider>();
            Mock<ICsvRowActivatorFactoryFactory<J139Report>> mockFactory
                = new Mock<ICsvRowActivatorFactoryFactory<J139Report>>();

            CachingDynamicCsvRowActivatorFactoryFactory<J139Report> sut
                = new CachingDynamicCsvRowActivatorFactoryFactory<J139Report>(mockFactory.Object);

            // Act
            sut.Create(mockConverterProvider.Object, mockColumnNameMapper.Object);

            // Assert
            mockFactory.Verify(
                f => f.Create(It.IsAny<IConverterProvider>(), It.IsAny<IColumnNameMapper>()),
                Times.Once()
            );
        }
        [TestMethod]
        public void PullsFromCacheSecondHit()
        {
            // Arrange
            Mock<IColumnNameMapper> mockColumnNameMapper = new Mock<IColumnNameMapper>();
            Mock<IConverterProvider> mockConverterProvider = new Mock<IConverterProvider>();
            Mock<ICsvRowActivatorFactoryFactory<J139Report>> mockFactory
                = new Mock<ICsvRowActivatorFactoryFactory<J139Report>>();

            CachingDynamicCsvRowActivatorFactoryFactory<J139Report> sut
                = new CachingDynamicCsvRowActivatorFactoryFactory<J139Report>(mockFactory.Object);

            // Act
            sut.Create(mockConverterProvider.Object, mockColumnNameMapper.Object);
            sut.Create(mockConverterProvider.Object, mockColumnNameMapper.Object);

            // Assert
            mockFactory.Verify(
                f => f.Create(It.IsAny<IConverterProvider>(), It.IsAny<IColumnNameMapper>()),
                Times.Once()
            );
        }
    }
}
