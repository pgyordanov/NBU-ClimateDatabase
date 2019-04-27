namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading
{
    using System;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.ClimateStation;

    public class ClimateStationReadingVM : IMapFrom<ClimateStationReading>
    {
        public ClimateStationVM ClimateStation { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public double ClimateStationIntervalWeight { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
