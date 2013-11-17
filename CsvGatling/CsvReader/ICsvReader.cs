using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvGatling
{
    public interface ICsvReader<T>
    {
        IEnumerable<T> ReadAll();
    }
}
