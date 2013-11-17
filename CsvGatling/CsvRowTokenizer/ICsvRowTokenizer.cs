using System;
using System.Collections.Generic;

namespace CsvGatling
{
    public interface ICsvRowTokenizer
    {
        IEnumerable<string[]> ReadAllRows();
    }
}
