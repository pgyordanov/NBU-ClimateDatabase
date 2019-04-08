using System;
using ClimateDatabase.Common.Mapping;

namespace ClimateDatabase.Web.Models
{
    public class ClimateStationReadingVM : IMapFrom<ClimateStationReadingVM>
    {
        public ClimateStationVM ClimateStation { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public double ClimateStationIntervalWeight { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}