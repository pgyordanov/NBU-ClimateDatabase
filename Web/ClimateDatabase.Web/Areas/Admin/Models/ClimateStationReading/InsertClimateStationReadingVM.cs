namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class InsertClimateStationReadingVM : IMapFrom<ClimateStationReading>
    {
        public string ClimateStationName { get; set; }

        [Required]
        public string ClimateStationId { get; set; }

        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }

        [Required]
        public double? AverageTemperature { get; set; }

        [Required]
        public double? TemperatureDeviation { get; set; }

        [Required]
        public double? MaximumTemperature { get; set; }

        [Required]
        public int? MaximumTemperatureDay { get; set; }

        [Required]
        public double? MinimumTemperature { get; set; }

        [Required]
        public int? MinimumTemperatureDay { get; set; }

        [Required]
        public double? RainSum { get; set; }

        [Required]
        public double? RainRatio { get; set; }

        [Required]
        public double? MaximumRain { get; set; }

        [Required]
        public int? MaximumRainDay { get; set; }

        [Required]
        public int? DaysWithRainMoreThan1mm { get; set; }

        [Required]
        public int? DaysWithRainMoreThan10mm { get; set; }

        [Required]
        public int? DaysWithWindFasterThan14ms { get; set; }

        [Required]
        public int? DaysWithThunder { get; set; }
    }
}
