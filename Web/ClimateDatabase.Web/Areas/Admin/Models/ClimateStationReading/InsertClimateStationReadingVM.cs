namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class InsertClimateStationReadingVM : IMapFrom<ClimateStationReading>
    {
        [Required]
        public string ClimateStationId { get; set; }

        [Required]
        [Range(1, 12)]
        public int FromMonth { get; set; }

        [Required]
        //[Range(0, DateTime.Now.Year)]
        public int FromYear { get; set; }

        [Required]
        [Range(1, 12)]
        public int ToMonth { get; set; }

        [Required]
        public int ToYear { get; set; }
    }
}
