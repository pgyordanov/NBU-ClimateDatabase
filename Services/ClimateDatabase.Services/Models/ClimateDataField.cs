namespace ClimateDatabase.Services.Models
{
    using System.ComponentModel;

    public enum ClimateDataField
    {
        [Description("Average temperature")]
        AverageTemperature = 1,
        [Description("Temperature deviation")]
        TemperatureDeviation,
        [Description("Maximum temperature")]
        MaximumTemperature,
        [Description("Maximum temperature for the day")]
        MaximumTemperatureDay,
        [Description("Mimimum temperature")]
        MinimumTemperature,
        [Description("Minimum temperature for the day")]
        MinimumTemperatureDay,
        [Description("Rain sum")]
        RainSum,
        [Description("Rain ratio")]
        RainRatio,
        [Description("Maximum rain")]
        MaximumRain,
        [Description("Maximum rain day")]
        MaximumRainDay,
        [Description("Days with more than 1mm")]
        DaysWithRainMoreThan1mm,
        [Description("Days with rain more than 10 mm")]
        DaysWithRainMoreThan10mm,
        [Description("Days with wind faster than 14 ms")]
        DaysWithWindFasterThan14ms,
        [Description("Days with tunder")]
        DaysWithThunder
    }
}