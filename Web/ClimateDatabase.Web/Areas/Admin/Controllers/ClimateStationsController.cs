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

            climateStationQuery = climateStationQuery.OrderByDescending(u => u.Name);

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
        [Route("admin/climate-stations/insert")]
        public async Task<IActionResult> InsertClimateStation(InsertClimateStationVM model)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddAlert(false, $"An error has occured while creating the station. Please try again.");
                return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
            }

            var climateStation = Mapper.Map<ClimateStation>(model);

            climateStation.CreatedOn = DateTime.Now;
            climateStation.ModifiedOn = DateTime.Now;

            await this.climateStationService.Create(climateStation);

            this.AddAlert(true, $"Station {model.Name} was successfully inserted.");

            return this.RedirectToAction("Index", new PaginationVM { ShowPage = 1, PageSize = 20 });
        }

        [HttpGet]
        [Route("admin/climate-stations/get")]
        public IActionResult GetClimateStations(string name)
        {
            var stations = this.climateStationService.GetAll()
                .Where(a => a.Name.ToLower().Contains(name.ToLower()))
                .Select(a => new { a.Id, a.Name })
                .ToList();

            return this.Json(stations);
        }
    }
}
