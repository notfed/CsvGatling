using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CsvGatling
{
    public class DefaultColumnNameMapper : IColumnNameMapper
    {
        public string GetColumnNameFor(PropertyInfo property)
        {
            return property.Name;
        }
    }
}
