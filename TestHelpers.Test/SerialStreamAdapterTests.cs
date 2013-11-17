using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelpers;

namespace TryStaticCsvGatling.Test
{
    [TestClass]
    public class SerialStreamAdapterTests
    {
        [TestMethod]
        public void ReturnsAppropriatelySizedChunks()
        {
            // Arrange
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write("abcdefghijklmnopqrstuvwxyz");
            streamWriter.Flush();
            memoryStream.Position = 0;
            int chunkSize = 2;

            SerialStreamAdapter sut = new SerialStreamAdapter(memoryStream, chunkSize);

            // Act/Assert
            byte[] buffer = new byte[4096];
            int count1 = sut.Read(buffer, 0, 4096);
            Assert.AreEqual('a', (char)buffer[0]);
            Assert.AreEqual('b', (char)buffer[1]);
            Assert.AreEqual(chunkSize, count1);
            int count2 = sut.Read(buffer, 0, 4096);
            Assert.AreEqual('c', (char)buffer[0]);
            Assert.AreEqual('d', (char)buffer[1]);
            Assert.AreEqual(chunkSize, count2);
            int count3 = sut.Read(buffer, 0, 4096);
            Assert.AreEqual('e', (char)buffer[0]);
            Assert.AreEqual('f', (char)buffer[1]);
            Assert.AreEqual(chunkSize, count3);
        }
    }
}
