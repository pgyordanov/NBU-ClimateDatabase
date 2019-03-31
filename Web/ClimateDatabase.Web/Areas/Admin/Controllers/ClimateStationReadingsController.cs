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

        //[HttpGet]
        //[Route("admin/climate-stations/{climateStationId}")]
        //public IActionResult ClimateStation(string climateStationId)
        //{
        //    if (string.IsNullOrWhiteSpace(climateStationId))
        //    {
        //        return this.NotFound($"invalid climate statation id");
        //    }

        //    return this.View();
        //}

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
