namespace ClimateDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ClimateDatabase.Data.Common.Models;

    public class ClimateStationReading : BaseKeylessDeletableModel
    {
        public virtual ClimateStation ClimateStation { get; set; }

        [Column(Order = 2)]
        public string ClimateStationId { get; set; }

        [Column(Order = 2)]
        public int Month { get; set; }

        [Column(Order = 2)]
        public int Year { get; set; }

        [Range(0, 1)]
        public double ClimateStationIntervalWeight { get; set; }

        public double? AverageTemperature { get; set; }

        public double? TemperatureDeviation { get; set; }

        public double? MaximumTemperature { get; set; }

        public int? MaximumTemperatureDay { get; set; }

        public double? MinimumTemperature { get; set; }

        public int? MinimumTemperatureDay { get; set; }

        public double? RainSum { get; set; }

        public double? RainRatio { get; set; }

        public double? MaximumRain { get; set; }

        public int? MaximumRainDay { get; set; }

        public int? DaysWithRainMoreThan1mm { get; set; }

        public int? DaysWithRainMoreThan10mm { get; set; }

        public int? DaysWithWindFasterThan14ms { get; set; }

        public int? DaysWithThunder { get; set; }
    }
}
