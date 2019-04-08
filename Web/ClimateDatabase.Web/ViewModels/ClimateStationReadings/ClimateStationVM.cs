using System;
using ClimateDatabase.Common.Mapping;

namespace ClimateDatabase.Web.Models
{
    public class ClimateStationVM : IMapFrom<ClimateStationVM>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Weight { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}