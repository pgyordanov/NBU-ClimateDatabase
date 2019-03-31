namespace ClimateDatabase.Services.WeightManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using ClimateDatabase.Common.Settings;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class WeightManager
    {
        private IOptions<ApplicationSettings> options;
        private ICrudService<ClimateStation> climateStationService;
        private IBaseCrudService<ClimateStationReading> climateStationReadingService;

        public WeightManager(
            IBaseCrudService<ClimateStationReading> climateStationReadingService,
            ICrudService<ClimateStation> climateStationService,
            IOptions<ApplicationSettings> options)
        {
            this.climateStationReadingService = climateStationReadingService;
            this.climateStationService = climateStationService;
            this.options = options;
        }

        public async Task ScaleStationGlobalWeight(ClimateStation station)
        {
            var stationReadingsGrouped = this.climateStationReadingService.GetAll()
                .Include(s => s.ClimateStation)
                .GroupBy(g => new { g.Month, g.Year })
                .Where(g => g.Any(s => s.ClimateStationId == station.Id))
                .ToList();

            foreach (var group in stationReadingsGrouped)
            {
                double stationsGlobalSum = group.Sum(s => s.ClimateStation.Weight).Value;
                double roundedSum = Math.Round(stationsGlobalSum, 2, MidpointRounding.AwayFromZero);

                foreach (var reading in group)
                {
                    var scaledWeight = Math.Round(reading.ClimateStation.Weight.Value / roundedSum, 2, MidpointRounding.AwayFromZero);
                    reading.ClimateStationIntervalWeight = scaledWeight;

                    await this.climateStationReadingService.Update(reading);
                }
            }
        }
    }
}
