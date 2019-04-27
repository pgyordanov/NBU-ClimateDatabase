namespace ClimateDatabase.Common.Settings
{
    public class ApplicationSettings
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public int ReadingsStartYear { get; set; }

        public double WeightSumErrorTreshold { get; set; }
    }
}
