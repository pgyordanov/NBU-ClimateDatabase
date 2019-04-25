using System.ComponentModel.DataAnnotations;

namespace ClimateDatabase.Services.Models
{
    public enum ClimateDataField
    {
        [Display(Name = "Average Temperature")]
        AverageTemperature,
        [Display(Name = "Temperature Deviations")]
        TemperatureDeviation,
        [Display(Name = "Maximum Temperature")]
        MaximumTemperature,
        [Display(Name = "Maximum Temperature Day")]
        MaximumTemperatureDay,
        [Display(Name = "Minimum Temperature")]
        MinimumTemperature,
        [Display(Name = "Minimum Temperature Day")]
        MinimumTemperatureDay,
        [Display(Name = "Rain Sum")]
        RainSum,
        [Display(Name = "Rain Ratio")]
        RainRatio,
        [Display(Name = "Maximum Rain")]
        MaximumRain,
        [Display(Name = "Maximum Rain Day")]
        MaximumRainDay,
        [Display(Name = "Days With Rain More Than 1mm")]
        DaysWithRainMoreThan1Mm,
        [Display(Name = "Days With Rain More Than 10mm")]
        DaysWithRainMoreThan10Mm,
        [Display(Name = "Days With Winds Faster Than 14ms")]
        DaysWithWindFasterThan14Ms,
        [Display(Name = "Days With Thunders")]
        DaysWithThunder
    }
}