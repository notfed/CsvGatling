using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace CsvGatling
{
    public class DefaultConverterProvider : IConverterProvider
    {
        public MethodInfo GetStringConverterTo(Type type)
        {
            if(type == typeof(string))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ConvertStringToString");
            }
            else if (type == typeof(byte))
            {
                return typeof(Convert)
                    .GetMethod("ToByte", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<byte>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(byte) });
            }
            else if (type == typeof(sbyte))
            {
                return typeof(Convert)
                    .GetMethod("ToSByte", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<sbyte>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(sbyte) });
            }
            else if (type == typeof(bool))
            {
                return typeof(Convert)
                    .GetMethod("ToBoolean", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<bool>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(bool) });
            }
            else if (type == typeof(char))
            {
                return typeof(Convert)
                    .GetMethod("ToChar", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<char>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(char) });
            }
            else if (type == typeof(Int32))
            {
                return typeof(Convert)
                    .GetMethod("ToInt32", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<Int32>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(Int32) });
            }
            else if (type == typeof(UInt32))
            {
                return typeof(Convert)
                    .GetMethod("ToUInt32", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<UInt32>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(UInt32) });
            }
            else if (type == typeof(Int16))
            {
                return typeof(Convert)
                    .GetMethod("ToInt16", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<Int16>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(Int16) });
            }
            else if (type == typeof(UInt16))
            {
                return typeof(Convert)
                    .GetMethod("ToUInt16", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<UInt16>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(UInt16) });
            }
            else if (type == typeof(long))
            {
                return typeof(Convert)
                    .GetMethod("ToInt64", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<long>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(long) });
            }
            else if (type == typeof(ulong))
            {
                return typeof(Convert)
                    .GetMethod("ToUInt64", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<ulong>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(ulong) });
            }
            else if (type == typeof(float))
            {
                return typeof(Convert)
                    .GetMethod("ToSingle", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<float>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(float) });
            }
            else if (type == typeof(Double))
            {
                return typeof(Convert)
                    .GetMethod("ToDouble", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<Double>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(Double) });
            }
            else if (type == typeof(Decimal))
            {
                return typeof(Convert)
                    .GetMethod("ToDecimal", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<Decimal>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(Decimal) });
            }
            else if (type == typeof(DateTime))
            {
                return typeof(Convert)
                    .GetMethod("ToDateTime", new Type[] { typeof(string) });
            }
            else if (type == typeof(Nullable<DateTime>))
            {
                return typeof(DefaultCustomConverters)
                    .GetMethod("ToNullable")
                    .MakeGenericMethod(new Type[] { typeof(DateTime) });
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
