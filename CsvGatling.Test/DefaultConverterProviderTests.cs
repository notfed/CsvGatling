using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGatling.Test
{
    [TestClass]
    public class DefaultConverterProviderTests
    {
        // Arrange
        private DefaultConverterProvider defaultConverterProvider = new DefaultConverterProvider();

        [TestMethod]
        public void Can_convert_from_string_to_bool()
        {
            bool result = (bool)defaultConverterProvider
                .GetStringConverterTo(typeof(bool))
                .Invoke(null, new object[] { "true" });
            Assert.AreEqual<bool>(true, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_bool()
        {
            bool? result = (bool?)defaultConverterProvider
                .GetStringConverterTo(typeof(bool?))
                .Invoke(null, new object[] { "true" });
            Assert.AreEqual<bool?>(true, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_byte()
        {
            byte result = (byte)defaultConverterProvider
                .GetStringConverterTo(typeof(byte))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<byte>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_byte()
        {
            byte? result = (byte?)defaultConverterProvider
                .GetStringConverterTo(typeof(byte?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<byte?>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_sbyte()
        {
            sbyte result = (sbyte)defaultConverterProvider
                .GetStringConverterTo(typeof(sbyte))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<sbyte>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_sbyte()
        {
            sbyte? result = (sbyte?)defaultConverterProvider
                .GetStringConverterTo(typeof(sbyte?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<sbyte?>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_char()
        {
            char result = (char)defaultConverterProvider
                .GetStringConverterTo(typeof(char))
                .Invoke(null, new object[] { "x" });
            Assert.AreEqual<char>('x', result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_char()
        {
            char? result = (char?)defaultConverterProvider
                .GetStringConverterTo(typeof(char?))
                .Invoke(null, new object[] { "x" });
            Assert.AreEqual<char?>('x', result);
        }

        [TestMethod]
        public void Can_convert_from_string_to_int()
        {
            int result = (int)defaultConverterProvider
                .GetStringConverterTo(typeof(int))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<int>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_int()
        {
            int? result = (int)defaultConverterProvider
                .GetStringConverterTo(typeof(int?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<int?>(5, result);
        }

        [TestMethod]
        public void Can_convert_from_string_to_uint()
        {
            uint result = (uint)defaultConverterProvider
                .GetStringConverterTo(typeof(uint))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<uint>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_uint()
        {
           uint? result = (uint)defaultConverterProvider
                .GetStringConverterTo(typeof(uint?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<uint?>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_short()
        {
            short result = (short)defaultConverterProvider
                .GetStringConverterTo(typeof(short))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<short>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_short()
        {
            short? result = (short)defaultConverterProvider
                .GetStringConverterTo(typeof(short?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<short?>(5, result);
        }

        [TestMethod]
        public void Can_convert_from_string_to_ushort()
        {
            ushort result = (ushort)defaultConverterProvider
                .GetStringConverterTo(typeof(ushort))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<ushort>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_ushort()
        {
            ushort? result = (ushort)defaultConverterProvider
                 .GetStringConverterTo(typeof(ushort?))
                 .Invoke(null, new object[] { "5" });
            Assert.AreEqual<ushort?>(5, result);
        }












        [TestMethod]
        public void Can_convert_from_string_to_long()
        {
            long result = (long)defaultConverterProvider
                .GetStringConverterTo(typeof(long))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<long>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_long()
        {
            long? result = (long?)defaultConverterProvider
                .GetStringConverterTo(typeof(long?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<long?>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_ulong()
        {
            ulong result = (ulong)defaultConverterProvider
                .GetStringConverterTo(typeof(ulong))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<ulong>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_ulong()
        {
            ulong? result = (ulong?)defaultConverterProvider
                .GetStringConverterTo(typeof(ulong?))
                .Invoke(null, new object[] { "5" });
            Assert.AreEqual<ulong?>(5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_double()
        {
            double result = (double)defaultConverterProvider
                .GetStringConverterTo(typeof(double))
                .Invoke(null, new object[] { "5.5" });
            Assert.AreEqual<double>(5.5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_double()
        {
            double? result = (double?)defaultConverterProvider
                .GetStringConverterTo(typeof(double?))
                .Invoke(null, new object[] { "5.5" });
            Assert.AreEqual<double?>(5.5, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_float()
        {
            float result = (float)defaultConverterProvider
                .GetStringConverterTo(typeof(float))
                .Invoke(null, new object[] { "5.5" });
            Assert.AreEqual<float>(5.5f, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_float()
        {
            float? result = (float?)defaultConverterProvider
                .GetStringConverterTo(typeof(float?))
                .Invoke(null, new object[] { "5.5" });
            Assert.AreEqual<float?>(5.5f, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_decimal()
        {
            decimal result = (decimal)defaultConverterProvider
                .GetStringConverterTo(typeof(decimal))
                .Invoke(null, new object[] { "5.5" });
            Assert.AreEqual<decimal>(5.5m, result);
        }
        [TestMethod]
        public void Can_convert_from_string_to_nullable_decimal()
        {
            decimal? result = (decimal?)defaultConverterProvider
                .GetStringConverterTo(typeof(decimal?))
                .Invoke(null, new object[] { "5.5" });
            Assert.AreEqual<decimal?>(5.5m, result);
        }

        [TestMethod]
        public void Can_convert_from_string_to_datetime()
        {
            DateTime result = (DateTime)defaultConverterProvider
                .GetStringConverterTo(typeof(DateTime))
                .Invoke(null, new object[] { "11/16/2013" });
            Assert.AreEqual<DateTime>(new DateTime(2013, 11, 16), result);
        }

        [TestMethod]
        public void Can_convert_from_string_to_nullable_datetime()
        {
            DateTime? result = (DateTime?)defaultConverterProvider
                .GetStringConverterTo(typeof(DateTime?))
                .Invoke(null, new object[] { "11/16/2013" });
            Assert.AreEqual<DateTime?>(new DateTime(2013, 11, 16), result);
        }

    }
}
