using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelpers;

namespace TryStaticCsvGatling.Test
{
    [TestClass]
    public class InfiniteCsvFileTests
    {
        [TestMethod]
        public void ReturnsCorrectHeaderAndValues()
        {
            string originalHeader = "BirthDate,Weight,ReportName\n";
            string originalValue = "3/5/2013,1,\"test\"\n";
            InfiniteCsvFile sut = new InfiniteCsvFile(originalHeader, originalValue);
            StreamReader sr = new StreamReader(sut);
            Assert.AreEqual("BirthDate,Weight,ReportName", sr.ReadLine());
            Assert.AreEqual("3/5/2013,1,\"test\"", sr.ReadLine());
            Assert.AreEqual("3/5/2013,1,\"test\"", sr.ReadLine());
            Assert.AreEqual("3/5/2013,1,\"test\"", sr.ReadLine());
            Assert.AreEqual("3/5/2013,1,\"test\"", sr.ReadLine());
        }
    }
}
