namespace ClimateDatabase.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using ClimateDatabase.Common.Extensions;
    using ClimateDatabase.Data.Common.Repositories;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class ClimateDataService : IClimateDataService
    {
        private IDeletableEntityRepository<ClimateStationReading> _climateSatiationReadings;

        public ClimateDataService(IDeletableEntityRepository<ClimateStationReading> climateSatiationReadings)
        {
            this._climateSatiationReadings = climateSatiationReadings;
        }

        public Dictionary<string, double> GetWeightedDataForPeriodByField(ClimateDataFilter filter)
        {
            var result = new Dictionary<string, double>();

            IQueryable<ClimateStationReading> climateStationReadings = this._climateSatiationReadings
                .AllAsNoTracking()
                .WhereIf(filter.From.HasValue, reading => new DateTime(reading.Year, reading.Month, 01) >= filter.From)
                .WhereIf(filter.To.HasValue, reading => new DateTime(reading.Year, reading.Month, 01) <= filter.To);

            IQueryable<IGrouping<DateTime, ClimateStationReading>> groupByYearAndMonth = climateStationReadings
                .GroupBy(reading => new DateTime(reading.Year, reading.Month, 01), reading => reading);

            foreach (var group in groupByYearAndMonth)
            {
                double dataWeighted = 0;
                double weightSum = group.Sum(x => x.ClimateStationIntervalWeight);

                foreach (ClimateStationReading reading in group)
                {
                    double weight = (weightSum == 0)? 0: reading.ClimateStationIntervalWeight / weightSum;
                    double fieldValue = GetFieldValue(reading, filter.ClimateDataField.ToString()) ?? 0;
                    dataWeighted += fieldValue * weight;
                }

                double resultValue = Math.Round(dataWeighted, 2);

                result.Add($"{group.Key:yyyy-MM}", resultValue);
            }

            return result;
        }

        public List<ClimateDataModelByDate> GetWeightedClimateDataForPeriodFull(ClimateDataFilter filter)
        {
            var result = new List<ClimateDataModelByDate>();

            IQueryable<ClimateStationReading> climateStationReadings = this._climateSatiationReadings
                .AllAsNoTracking()
                .WhereIf(filter.From.HasValue, reading => new DateTime(reading.Year, reading.Month, 01) >= filter.From)
                .WhereIf(filter.To.HasValue, reading => new DateTime(reading.Year, reading.Month, 01) <= filter.To)
                .Include(x => x.ClimateStation);

            IQueryable<IGrouping<DateTime, ClimateStationReading>> groupByYearAndMonth = climateStationReadings
                .GroupBy(reading => new DateTime(reading.Year, reading.Month, 01), reading => reading);

            foreach (var group in groupByYearAndMonth)
            {
                var resultValue = new ClimateDataModelByDate()
                {
                    Date = $"{group.Key:yyyy-MM}"
                };
                
                double weightSum = group.Sum(x => x.ClimateStationIntervalWeight);
                IEnumerable<FieldInfo> fields = resultValue.GetType().GetFields().Where(x => x.Name != nameof(ClimateDataModelByDate.Date)).ToList();

                foreach (FieldInfo field in fields)
                {
                    double dataWeighted = 0;

                    foreach (ClimateStationReading reading in group)
                    {
                        double weight = reading.ClimateStationIntervalWeight / weightSum;
                        double fieldValue = GetFieldValue(reading, field.Name) ?? 0;
                        dataWeighted += fieldValue * weight;
                    }

                    string fieldWeightedValue = Math.Round(dataWeighted, 2).ToString(CultureInfo.InvariantCulture);
                    field.SetValue(field, fieldWeightedValue);
                }

                result.Add(resultValue);
            }

            return result;
        }

        public async Task<List<ClimateDataModelByStation>> GetClimateDataByStation(ClimateDataFilter filter)
        {
            var climateStationReadings = this._climateSatiationReadings
                .AllAsNoTracking()
                .WhereIf(filter.From.HasValue, reading => new DateTime(reading.Year, reading.Month, 01) >= filter.From)
                .WhereIf(filter.To.HasValue, reading => new DateTime(reading.Year, reading.Month, 01) <= filter.To)
                .Include(x => x.ClimateStation);

            IQueryable<IGrouping<string, ClimateStationReading>> groupByStation = climateStationReadings
                .GroupBy(reading => reading.ClimateStationId, reading => reading);

            return await groupByStation.Select(
                    x => new ClimateDataModelByStation()
                    {
                        ClimateStationName = x.FirstOrDefault().ClimateStation.Name,
                        RainSum = x.Average(y => y.RainSum).ToString(),
                        AverageTemperature = x.Average(y => y.AverageTemperature).ToString(),
                        TemperatureDeviation = x.Average(y => y.TemperatureDeviation).ToString(),
                        MaximumTemperature = x.Average(y => y.MaximumTemperature).ToString(),
                        MaximumTemperatureDay = x.Average(y => y.MaximumTemperatureDay).ToString(),
                        MinimumTemperature = x.Average(y => y.MinimumTemperature).ToString(),
                        MinimumTemperatureDay = x.Average(y => y.MinimumTemperatureDay).ToString(),
                        RainRatio = x.Average(y => y.RainRatio).ToString(),
                        MaximumRain = x.Average(y => y.MaximumRain).ToString(),
                        MaximumRainDay = x.Average(y => y.MaximumRainDay).ToString(),
                        DaysWithThunder = x.Average(y => y.DaysWithThunder).ToString(),
                        DaysWithRainMoreThan1mm = x.Average(y => y.DaysWithRainMoreThan1mm).ToString(),
                        DaysWithRainMoreThan10mm = x.Average(y => y.DaysWithRainMoreThan10mm).ToString(),
                        DaysWithWindFasterThan14ms = x.Average(y => y.DaysWithWindFasterThan14ms).ToString()
                    })
                .ToListAsync();
        }

        private static double? GetFieldValue(ClimateStationReading reading, string field)
        {
            object fieldValue = reading.GetType().GetProperty(field)?.GetValue(reading, null);

            return fieldValue as double?;
        }
    }
}