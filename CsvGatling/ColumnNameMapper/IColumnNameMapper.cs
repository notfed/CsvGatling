using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CsvGatling
{
    public interface IColumnNameMapper
    {
        string GetColumnNameFor(PropertyInfo property);
    }
}
