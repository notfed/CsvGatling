using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CsvGatling
{
    /// <summary>
    /// Reads rows from a stream and maps rows to a list of objects of type <typeparamref name="T"/>
    /// </summary>
    public class CsvReader<T> : ICsvReader<T>
    {
        // Singleton provider of activator factories
        private readonly static ICsvRowActivatorFactoryFactory<T> activatorFactoryFactory
         = new CachingDynamicCsvRowActivatorFactoryFactory<T>(
             new DynamicCsvRowActivatorFactoryFactory<T>()
           );

        private readonly Stream csvStream;
        public CsvReader(Stream csvInputStream)
        {
            if (csvInputStream == null)
            {
                throw new ArgumentNullException("csvInputStream");
            }
            csvStream = csvInputStream;
            
        }

        public IEnumerable<T> ReadAll()
        {
            // Create an activator factory
            ICsvRowActivatorFactory<T> activatorFactory = activatorFactoryFactory.Create(
                new DefaultConverterProvider(),
                new DefaultColumnNameMapper()
            );

            // Create a tokenizer that reads from the csvStream
            CsvRowTokenizer tokenizer = new CsvRowTokenizer(csvStream);

            // Start streaming data from the tokenizer
            IEnumerable<string[]> rows = tokenizer.ReadAllRows();

            // Grab the first line (the header)
            IEnumerator<string[]> enumerator = rows.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new InvalidDataException("No header found!");
            }
            string[] headerrow = enumerator.Current;
            int columnCount = headerrow.Length;

            // Start streaming tokenized data into the activator
            ICsvRowActivator<T> activator = activatorFactory.Create(headerrow);

            // Stream out all of the CSV rows as objects
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Length != columnCount)
                {
                    throw new InvalidDataException(
                        string.Format("Field count mismatch: a row had {0} field(s), but header had {1}!", enumerator.Current.Length, columnCount)
                    );
                }
                yield return activator.CreateFromRow(enumerator.Current);
            }
        }
    }
}
