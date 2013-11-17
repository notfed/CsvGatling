using System;

namespace CsvGatling
{
    /// <summary>
    /// Reads reads an array of strings and maps the values to an object
    /// </summary>
    public interface ICsvRowActivator<T>
    {
        T CreateFromRow(string[] csvValue);
    }
}
