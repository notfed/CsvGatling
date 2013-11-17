using System;

namespace CsvGatling
{
    public interface ICsvRowActivatorFactoryFactory<T>
    {
        ICsvRowActivatorFactory<T> Create(IConverterProvider converterProvider, IColumnNameMapper columnNameMapper);
    }
}
