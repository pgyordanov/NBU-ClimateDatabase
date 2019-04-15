namespace ClimateDatabase.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ClimateDatabase.Common.Extensions;
    using ClimateDatabase.Data.Common.Repositories;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Services.Models;

    public class ClimateDataService : IClimateDataService
    {
        private IDeletableEntityRepository<ClimateStationReading> _climateSatiationReadings;

        public ClimateDataService(IDeletableEntityRepository<ClimateStationReading> climateSatiationReadings)
        {
            this._climateSatiationReadings = climateSatiationReadings;
        }

        public Dictionary<string, double> GetClimateDataForPeriodByField(ClimateDataFilter filter)
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
                    double weight = reading.ClimateStationIntervalWeight / weightSum;
                    double fieldValue = GetFieldValue(reading, filter.ClimateDataField) ?? 0;
                    dataWeighted += fieldValue * weight;
                }

                double resultValue = Math.Round(dataWeighted, 2);

                result.Add($"{group.Key:yyyy-MM}", resultValue);
            }

            return result;
        }

        private static double? GetFieldValue(ClimateStationReading reading, ClimateDataField field)
        {
            object fieldValue = reading.GetType().GetProperty(field.ToString())?.GetValue(reading, null);

            return fieldValue as double?;
        }
    }
}