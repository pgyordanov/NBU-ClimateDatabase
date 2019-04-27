namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStation
{
    using System;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class ClimateStationExtendedVM : IMapFrom<ClimateStation>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
