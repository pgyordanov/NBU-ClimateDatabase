namespace ClimateDatabase.Services.Models
{
    using System;

    public class ClimateDataFilter
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public ClimateDataField ClimateDataField { get; set; }
    }
}