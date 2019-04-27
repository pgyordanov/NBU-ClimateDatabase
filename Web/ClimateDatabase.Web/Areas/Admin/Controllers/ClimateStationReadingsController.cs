namespace ClimateDatabase.Web.Areas.Admin.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ClimateDatabase.Common.Settings;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Web.Areas.Admin.Controllers.Base;
    using ClimateDatabase.Web.Areas.Admin.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class ClimateStationReadingsController : EntityListController
    {
        private IOptions<ApplicationSettings> options;
        private IBaseCrudService<ClimateStationReading> climateStationReadingService;


        public ClimateStationReadingsController(IBaseCrudService<ClimateStationReading> climateStationReadingService, IOptions<ApplicationSettings> options)
        {
            this.climateStationReadingService = climateStationReadingService;
            this.options = options;
        }

        [HttpGet]
        [Route("admin/climate-station-readings")]
        public IActionResult Index(PaginationVM pagination, string climateStationName, string fromPeriod, string toPeriod)
        {
            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            var climateStationReadingQuery = this.climateStationReadingService.GetAll().Include(cr => cr.ClimateStation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(climateStationName))
            {
                climateStationReadingQuery = climateStationReadingQuery.Where(a => a.ClimateStation.Name.ToLower().Contains(climateStationName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(fromPeriod))
            {
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
                DateTime period;
                bool result = DateTime.TryParseExact(toPeriod, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out period);

                if (result)
                {
                    climateStationReadingQuery = climateStationReadingQuery
                        .Where(a => DateTime.ParseExact(a.Month.ToString("00") + "-" + a.Year, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None) <= period);
                }
            }

            climateStationReadingQuery = climateStationReadingQuery.OrderBy(u => u.Year).ThenBy(u => u.Month).ThenBy(u => u.ClimateStation.Name);

            var paginatedReadings = this.PaginateList<ClimateStationReadingVM>(pagination, climateStationReadingQuery.ProjectTo<ClimateStationReadingVM>()).ToList();

            int totalPages = this.GetTotalPages(pagination.PageSize, climateStationReadingQuery.Count());

            ClimateStationReadingListVM climateStationReadingsModel = new ClimateStationReadingListVM
            {
                ClimateStationReadings = paginatedReadings,
                NextPage = pagination.ShowPage < totalPages ? pagination.ShowPage + 1 : pagination.ShowPage,
                PreviousPage = pagination.ShowPage > 1 ? pagination.ShowPage - 1 : pagination.ShowPage,
                CurrentPage = pagination.ShowPage,
                TotalPages = totalPages,
                ShowPagination = totalPages > 1,
            };

            return this.View(climateStationReadingsModel);
        }

        [HttpGet]
        [Route("admin/climate-station-readings/{climateStationId}/{month}/{year}")]
        public IActionResult ClimateStationReading(string climateStationId, int month, int year)
        {
            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            if (string.IsNullOrWhiteSpace(climateStationId))
            {
                return this.NotFound($"invalid climate statation id");
            }

            var climateStationReading = this.climateStationReadingService.GetAll()
                .Include(r => r.ClimateStation)
                .FirstOrDefault(r => r.ClimateStationId == climateStationId && r.Month == month && r.Year == year);

            if (climateStationReading == null)
            {
                return this.NotFound($"reading not found");
            }

            var climateStationReadingModel = Mapper.Map<ClimateStationReadingFullVM>(climateStationReading);

            return this.View(climateStationReadingModel);
        }


        [HttpPost]
        [Route("admin/climate-station-readings/{climateStationId}/{month}/{year}")]
        public async Task<IActionResult> ClimateStationReading(ClimateStationReadingFullVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddAlert(false, $"An error has occured while updating station reading. Please try again.");
                return this.View(model);
            }

            var climateStationReading = await this.climateStationReadingService.Get(model.ClimateStationId, model.Year, model.Month);

            climateStationReading.AverageTemperature = model.AverageTemperature;
            climateStationReading.MaximumTemperature = model.MaximumTemperature;
            climateStationReading.MaximumTemperatureDay = model.MaximumTemperatureDay;
            climateStationReading.MinimumTemperature = model.MinimumTemperature;
            climateStationReading.MinimumTemperatureDay = model.MinimumTemperatureDay;
            climateStationReading.RainSum = model.RainSum;
            climateStationReading.RainRatio = model.RainRatio;
            climateStationReading.MaximumRain = model.MaximumRain;
            climateStationReading.MaximumRainDay = model.MaximumRainDay;
            climateStationReading.DaysWithRainMoreThan1mm = model.DaysWithRainMoreThan1mm;
            climateStationReading.DaysWithRainMoreThan10mm = model.DaysWithRainMoreThan10mm;
            climateStationReading.DaysWithThunder = model.DaysWithThunder;
            climateStationReading.DaysWithWindFasterThan14ms = climateStationReading.DaysWithWindFasterThan14ms;

            climateStationReading.ModifiedOn = DateTime.Now;

            await this.climateStationReadingService.Update(climateStationReading);

            this.AddAlert(true, "Successfully updated climate station reading");
            return this.RedirectToAction("ClimateStationReading", new {climateStationId = model.ClimateStationId, month = model.Month, year = model.Year });
        }

        [HttpPost]
        [Route("admin/climate-station-readings/insert")]
        public async Task<IActionResult> InsertClimateStationReading(InsertClimateStationReadingVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddAlert(false, $"An error has occured while creating the station reading. Please try again.");
                return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
            }

            DateTime fromPeriod;
            bool resultFromPeriod = DateTime.TryParseExact(model.FromDate, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromPeriod);

            DateTime toPeriod;
            bool resultToPeriod = DateTime.TryParseExact(model.ToDate, "MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out toPeriod);

            if (fromPeriod.Year < this.options.Value.ReadingsStartYear || toPeriod.Year < this.options.Value.ReadingsStartYear)
            {
                this.AddAlert(false, $"Cannot insert readings prior to 01/{this.options.Value.ReadingsStartYear}. Please try again.");
                return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
            }

            DateTime currentPeriod = fromPeriod;
            while (currentPeriod <= toPeriod)
            {
                var climateStationReading = Mapper.Map<ClimateStationReading>(model);

                climateStationReading.ClimateStation = null;
                climateStationReading.Year = currentPeriod.Year;
                climateStationReading.Month = currentPeriod.Month;
                climateStationReading.ClimateStationIntervalWeight = 0;

                climateStationReading.CreatedOn = DateTime.Now;
                climateStationReading.ModifiedOn = DateTime.Now;

                await this.climateStationReadingService.Create(climateStationReading);

                currentPeriod = currentPeriod.AddMonths(1);
            }

            this.AddAlert(true, $"Reading was successfully inserted.");

            return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
        }
    }
}
