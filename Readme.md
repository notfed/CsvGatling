CsvGatling
------------------

CsvGatling is a fast CSV parser with a simple C# interface.  

The `CsvReader` class will read and parse a CSV file stream, and for each row it finds, it will return an object with fields mapped to properties.  

Minimal Example
------------------

Imagine we have this CSV file:

    Name,FavoriteColor,BirthDate
    John,Red,11/2/1980
    Ted,Green,4/3/1989
    Chris,Blue,11/10/2000

We'll create a class to map each row to:

    public class Person
    {
        public DateTime BirthDate { get; set; }
        public string Name { get; set; }
        public string FavoriteColor { get; set; }
    }

Then, to read the file:

    using System;
    using System.IO;
    using CsvGatling;

    using (FileStream fileStream = new FileStream(@"C:\Path\To\CSV\File", FileMode.Open))
    {
        ICsvReader<Person> csvReader = new CsvReader<Person>(fileStream);
        foreach (Person person in csvReader.ReadAll())
        {
            Console.WriteLine("{0} was born on {1:M/d/yyyy}, and his favorite color is {2}",
                person.Name,
                person.BirthDate,
                person.FavoriteColor);
                                   
        }
     }

The output will be:

    John was born on 11/2/1980, and his favorite color is Red
    Ted was born on 4/3/1989, and his favorite color is Green
    Chris was born on 11/10/1900, and his favorite color is Blue


Design
------------------


CsvGatling...
========


...is simple.  As shown in the example above, its very simple interface makes it extremely easy to use.

...is streaming.  It will not try to read the entire file into memory; it will only read rows into memory as they are being read.  

...is scalable.  There are no size limits, and the library can easily read CSV files of any number of rows and any number of columns.

...is fast.  It uses `System.Reflection` to map column names to properties with the same name, and uses `System.Reflection.Emit` to dynamically generate code to avoid the need to use Reflection for each row.

...follows the [RFC 4180](http://tools.ietf.org/html/rfc4180) standard, with a few caveats.  By default, fields are comma separated, lines are new line separated, quotes use double-quotes, and quotes are escaped by having two neighboring quotes, just as the standard says.  (The caveats are: (1) CsvGatling will work for both UNIX (`\n`) and Windows (`\r\n`) line endings. (2) Files *must* have a header. (3) Users can choose to use different column delimiter and quote characters with the appropriate method overloads.)

Author
------------------
Jay Sullivan (jay@petio.org)

License
------------------
Public Domain
