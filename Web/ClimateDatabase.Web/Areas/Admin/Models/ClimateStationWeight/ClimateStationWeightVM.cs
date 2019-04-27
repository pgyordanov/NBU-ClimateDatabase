namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationWeight
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class ClimateStationWeightVM : IMapFrom<ClimateStationReading>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 1)]
        public double? Weight { get; set; }
    }
}
