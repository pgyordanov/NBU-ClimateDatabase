namespace ClimateDatabase.Web.Areas.Admin.Controllers
{
    using System.Linq;

    using AutoMapper.QueryableExtensions;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Web.Areas.Admin.Controllers.Base;
    using ClimateDatabase.Web.Areas.Admin.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.Base;
    using ClimateDatabase.Web.Areas.Admin.Models.ClimateStation;

    using Microsoft.AspNetCore.Mvc;

    public class ClimateStationsController : EntityListController
    {
        private ICrudService<ClimateStation> climateStationService;

        public ClimateStationsController(ICrudService<ClimateStation> climateStationService)
        {
            this.climateStationService = climateStationService;
        }

        [HttpGet]
        [Route("admin/climate-stations")]
        public IActionResult Index(PaginationVM pagination, string climateStationName)
        {
            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            var climateStationQuery = this.climateStationService.GetAllWithDeleted();

            if (!string.IsNullOrWhiteSpace(climateStationName))
            {
                climateStationQuery = climateStationQuery.Where(a => a.Name.ToLower().Contains(climateStationName.ToLower()));
            }

            climateStationQuery = climateStationQuery.OrderBy(u => u.IsDeleted).ThenByDescending(u => u.Name);

            var paginatedStations = this.PaginateList<ClimateStationVM>(pagination, climateStationQuery.ProjectTo<ClimateStationVM>()).ToList();

            int totalPages = this.GetTotalPages(pagination.PageSize, climateStationQuery.Count());

            ClimateStationListVM climateStationsModel = new ClimateStationListVM
            {
                ClimateStations = paginatedStations,
                NextPage = pagination.ShowPage < totalPages ? pagination.ShowPage + 1 : pagination.ShowPage,
                PreviousPage = pagination.ShowPage > 1 ? pagination.ShowPage - 1 : pagination.ShowPage,
                CurrentPage = pagination.ShowPage,
                TotalPages = totalPages,
                ShowPagination = totalPages > 1,
            };

            return this.View(climateStationsModel);
        }

        [HttpGet]
        [Route("admin/climate-stations/{climateStationId}")]
        public IActionResult ClimateStation(string climateStationId)
        {
            if (string.IsNullOrWhiteSpace(climateStationId))
            {
                return this.NotFound($"invalid climate statation id");
            }

            return this.View();
        }

        [HttpPost]
        [Route("admin/climate-stations/insert")]
        public IActionResult InsertClimateStation(InsertClimateStationVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddAlert(false, $"An error has occured while creating the station. Please try again.");
                return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
            }

            return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
        }
    }
}