namespace ClimateDatabase.Services.Models
{
    public class ClimateDataModelByDate
    {
        public string Date { get; set; }
        
        public string AverageTemperature { get; set; }

        public string TemperatureDeviation { get; set; }

        public string MaximumTemperature { get; set; }

        public string MaximumTemperatureDay { get; set; }

        public string MinimumTemperature { get; set; }

        public string MinimumTemperatureDay { get; set; }

        public string RainSum { get; set; }

        public string RainRatio { get; set; }

        public string MaximumRain { get; set; }

        public string MaximumRainDay { get; set; }

        public string DaysWithRainMoreThan1mm { get; set; }

        public string DaysWithRainMoreThan10mm { get; set; }

        public string DaysWithWindFasterThan14ms { get; set; }

        public string DaysWithThunder { get; set; }
    }
}