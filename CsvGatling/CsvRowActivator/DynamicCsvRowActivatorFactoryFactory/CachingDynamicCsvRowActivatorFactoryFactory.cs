using System;
using System.Collections.Generic;

namespace CsvGatling
{
    public class CachingDynamicCsvRowActivatorFactoryFactory<T> : ICsvRowActivatorFactoryFactory<T>
    {
        private struct CachingKey 
        {
            public readonly IConverterProvider ConverterProvider;
            public readonly IColumnNameMapper ColumnNameMapper;
            public CachingKey(IConverterProvider converterProvider, IColumnNameMapper columnNameMapper)
	        {
                this.ConverterProvider = converterProvider;
                this.ColumnNameMapper = columnNameMapper;
	        }
        }

        private IDictionary<CachingKey,ICsvRowActivatorFactory<T>> cache;
        private readonly ICsvRowActivatorFactoryFactory<T> factoryFactory;

        public CachingDynamicCsvRowActivatorFactoryFactory(ICsvRowActivatorFactoryFactory<T> factoryFactory)
        {
            this.cache = new Dictionary<CachingKey, ICsvRowActivatorFactory<T>>();
            this.factoryFactory = factoryFactory;
        }
        public ICsvRowActivatorFactory<T> Create(IConverterProvider converterProvider, IColumnNameMapper columnNameMapper)
        {
            CachingKey key = new CachingKey(converterProvider, columnNameMapper);
            if (!cache.Keys.Contains(key))
            {
                var newFactory = factoryFactory.Create(converterProvider, columnNameMapper);
                cache.Add(key, newFactory);
                return newFactory;
            }
            else
            {
                return cache[key];
            }
        }
    }
}
