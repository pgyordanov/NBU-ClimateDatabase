namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStation
{
    using System;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class ClimateStationVM : IMapFrom<ClimateStation>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Weight { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
