using System;

namespace CsvGatling.Test
{
    public class J139Report
    {
        public int ClientId { get; set; }
        public DateTime BirthDate { get; set; }
        public double   Weight      { get; set; }
        public string   ReportName      { get; set; }
    }
    public class J139ReportWithNullables
    {
        public int? ClientId { get; set; }
        public DateTime? BirthDate { get; set; }
        public double? Weight { get; set; }
        public string ReportName { get; set; }
    }
}
