namespace ClimateDatabase.Web.ViewModels.Export
{
    using System;
    using ClimateDatabase.Services.Models;

    public class ExportFilter
    {
        public ExportType ExportType { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public ClimateDataField? ClimateDataField { get; set; }
    }
}