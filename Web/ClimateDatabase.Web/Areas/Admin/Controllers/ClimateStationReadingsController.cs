namespace ClimateDatabase.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Web.Areas.Admin.Controllers.Base;
    using ClimateDatabase.Web.Areas.Admin.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ClimateStationReadingsController : EntityListController
    {
        private IBaseCrudService<ClimateStationReading> climateStationReadingService;

        public ClimateStationReadingsController(IBaseCrudService<ClimateStationReading> climateStationReadingService)
        {
            this.climateStationReadingService = climateStationReadingService;
        }

        [HttpGet]
        [Route("admin/climate-station-readings")]
        public IActionResult Index(PaginationVM pagination, string climateStationName)
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

            climateStationReadingQuery = climateStationReadingQuery.OrderByDescending(u => u.ClimateStation.Name);

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

        //[HttpPost]
        //[Route("admin/climate-stations/insert")]
        //public async Task<IActionResult> InsertClimateStation(InsertClimateStationVM model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        this.AddAlert(false, $"An error has occured while creating the station. Please try again.");
        //        return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
        //    }

        //    var climateStation = Mapper.Map<ClimateStation>(model);

        //    climateStation.CreatedOn = DateTime.Now;
        //    climateStation.ModifiedOn = DateTime.Now;

        //    await this.climateStationReadingService.Create(climateStation);

        //    this.AddAlert(true, $"Station {model.Name} was successfully inserted.");

        //    return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
        //}
    }
}
