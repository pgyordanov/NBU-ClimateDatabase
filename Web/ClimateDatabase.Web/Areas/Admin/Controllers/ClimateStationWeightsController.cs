namespace ClimateDatabase.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ClimateDatabase.Common.Settings;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Services.WeightManager;
    using ClimateDatabase.Web.Areas.Admin.Controllers.Base;
    using ClimateDatabase.Web.Areas.Admin.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading;
    using ClimateDatabase.Web.Areas.Admin.Models.ClimateStationWeight;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class ClimateStationWeightsController : BaseController
    {
        private IOptions<ApplicationSettings> options;
        private ICrudService<ClimateStation> climateStationService;
        private IBaseCrudService<ClimateStationReading> climateStationReadingService;
        private WeightManager weightManager;

        public ClimateStationWeightsController(
            IBaseCrudService<ClimateStationReading> climateStationReadingService,
            ICrudService<ClimateStation> climateStationService,
            IOptions<ApplicationSettings> options,
            WeightManager weightManager)
        {
            this.climateStationReadingService = climateStationReadingService;
            this.climateStationService = climateStationService;
            this.options = options;
            this.weightManager = weightManager;
        }

        [HttpGet]
        [Route("admin/climate-station-weights")]
        public IActionResult Index()
        {
            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            return this.View();
        }

        [HttpGet]
        [Route("admin/climate-station-weights/global")]
        public IActionResult GlobalWeights()
        {
            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            var climateStations = this.climateStationService.GetAll().OrderByDescending(c => c.Name);
            var climateStationWeightsModel = climateStations.ProjectTo<ClimateStationWeightVM>().ToList();

            var model = new ClimateStationWeightListVM
            {
                ClimateStationWeights = climateStationWeightsModel
            };

            return this.View(model);
        }

        [HttpPost]
        [Route("admin/climate-station-weights/global")]
        public async Task<IActionResult> GlobalWeights(ClimateStationWeightListVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddAlert(false, $"An error has occured while updating station weights. Please try again.");
                return this.RedirectToAction("GlobalWeights");
            }

            double weightSum = 0;
            foreach (var station in model.ClimateStationWeights)
            {
                weightSum += station.Weight.Value;
            }

            if (weightSum != 0 && Math.Abs(1 - weightSum) > this.options.Value.WeightSumErrorTreshold)
            {
                this.ModelState.AddModelError("sum-error", "Station weights must equal 1");
                return this.View(model);
            }

            List<ClimateStation> stationsForScaling = new List<ClimateStation>();

            foreach (var station in model.ClimateStationWeights)
            {
                var stationDo = await this.climateStationService.Get(station.Id);

                if (stationDo != null)
                {
                    if (station.Weight != stationDo.Weight || model.RecalculatePeriodWeights)
                    {
                        stationsForScaling.Add(stationDo);

                        stationDo.Weight = station.Weight;
                        stationDo.ModifiedOn = DateTime.Now;

                        await this.climateStationService.Update(stationDo);
                    }
                }
            }

            foreach (var stationDo in stationsForScaling)
            {
                await this.weightManager.ScaleStationGlobalWeight(stationDo);
            }

            this.AddAlert(true, $"Global weights were successfully set Monthly weights have been scaled for the updated stations.");

            return this.RedirectToAction("GlobalWeights");
        }

        [HttpGet]
        [Route("admin/climate-station-weights/period")]
        public IActionResult PeriodWeights(string fromPeriod, string toPeriod)
        {
            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            bool showQuery = false;
            var climateStationReadingQuery = this.climateStationReadingService.GetAll()
                .Include(s => s.ClimateStation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(fromPeriod))
            {
                showQuery = true;

                DateTime period;
                bool result = DateTime.TryParseExact(fromPeriod, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out period);

                if (result)
                {
                    climateStationReadingQuery = climateStationReadingQuery
                        .Where(a => DateTime.ParseExact(a.Month.ToString("00") + "-" + a.Year, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None) >= period);
                }
            }

            if (!string.IsNullOrWhiteSpace(toPeriod))
            {
                showQuery = true;

                DateTime period;
                bool result = DateTime.TryParseExact(toPeriod, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out period);

                if (result)
                {
                    climateStationReadingQuery = climateStationReadingQuery
                        .Where(a => DateTime.ParseExact(a.Month.ToString("00") + "-" + a.Year, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None) <= period);
                }
            }

            if (showQuery)
            {
                climateStationReadingQuery = climateStationReadingQuery.OrderBy(u => u.Year).ThenBy(u => u.Month).ThenBy(u => u.ClimateStation.Name);
                var climateStationWeightsModel = climateStationReadingQuery.ProjectTo<ClimateStationReadingWeightVM>().ToList();

                var model = new ClimateStationReadingWeightListVM
                {
                    ClimateStationReadingWeights = climateStationWeightsModel
                };

                return this.View(model);
            }

            return this.View(null);
        }

        [HttpPost]
        [Route("admin/climate-station-weights/period")]
        public async Task<IActionResult> PeriodWeights(ClimateStationReadingWeightListVM model, string fromPeriod, string toPeriod)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddAlert(false, $"An error has occured while updating station weights. Please try again.");
                return this.RedirectToAction("PeriodWeights", new { fromPeriod, toPeriod });
            }

            int currentMonth = 0, currentYear = 0;
            double weightSum = 0;
            foreach (var reading in model.ClimateStationReadingWeights)
            {
                if (currentMonth != reading.Month || currentYear != reading.Year)
                {
                    if (weightSum != 0 && Math.Abs(1 - weightSum) > this.options.Value.WeightSumErrorTreshold)
                    {
                        this.ModelState.AddModelError("sum-error", $"Station weights must equal 1. See {currentMonth}-{currentYear}");
                        return this.View(model);
                    }

                    weightSum = 0;
                    currentMonth = reading.Month;
                    currentYear = reading.Year;
                }

                weightSum += reading.ClimateStationIntervalWeight.Value;
            }

            foreach (var reading in model.ClimateStationReadingWeights)
            {
                var readingDo = await this.climateStationReadingService.Get(reading.ClimateStationId, reading.Year, reading.Month);

                if (readingDo != null)
                {
                    if (reading.ClimateStationIntervalWeight != readingDo.ClimateStationIntervalWeight)
                    {
                        readingDo.ClimateStationIntervalWeight = reading.ClimateStationIntervalWeight.Value;
                        readingDo.ModifiedOn = DateTime.Now;

                        await this.climateStationReadingService.Update(readingDo);
                    }
                }
            }

            this.AddAlert(true, $"Weights for period were successfully set.");

            return this.RedirectToAction("PeriodWeights", new { fromPeriod, toPeriod });
        }
    }
}
