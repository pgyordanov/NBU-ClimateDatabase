namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationWeight
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class ClimateStationReadingWeightVM : IMapFrom<ClimateStationReading>
    {
        [Required]
        public string ClimateStationId { get; set; }

        public ClimateStationWeightVM ClimateStation { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [Range(0, 1)]
        public double? ClimateStationIntervalWeight { get; set; }
    }
}
