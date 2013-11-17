using System;

namespace CsvGatling
{
    public interface ICsvRowActivatorFactory<T>
    {
        ICsvRowActivator<T> Create(string[] headerrow);
    }
}
