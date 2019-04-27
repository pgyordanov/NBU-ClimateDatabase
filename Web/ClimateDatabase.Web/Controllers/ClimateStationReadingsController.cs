using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClimateDatabase.Common.Settings;
using ClimateDatabase.Data.Models;
using ClimateDatabase.Services.Contracts;
using ClimateDatabase.Web.Controllers.Base;
using ClimateDatabase.Web.Models;
using ClimateDatabase.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClimateDatabase.Web.Controllers
{
    public class ClimateStationReadingsController : EntityListController
    {
        private readonly IBaseCrudService<ClimateStationReading> _climateStationReadingService;
        private IOptions<ApplicationSettings> options;

        public ClimateStationReadingsController(IBaseCrudService<ClimateStationReading> climateStationService,
            IOptions<ApplicationSettings> options)
        {
            _climateStationReadingService = climateStationService;
            this.options = options;
        }

        [HttpGet]
        [Route("/readings")]
        public IActionResult Index(
            PaginationVM pagination,
            string climateStationName,
            string fromPeriod,
            string toPeriod)
        {
            if (HasAlert) SetAlertModel();

            var climateStationReadingQuery =
                _climateStationReadingService.GetAll().Include(cr => cr.ClimateStation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(climateStationName))
                climateStationReadingQuery = climateStationReadingQuery.Where(a =>
                    a.ClimateStation.Name.ToLower().Contains(climateStationName.ToLower()));

            if (!string.IsNullOrWhiteSpace(fromPeriod))
            {
                var result = DateTime.TryParseExact(fromPeriod, "MM-yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var period);

                if (result)
                    climateStationReadingQuery = climateStationReadingQuery
                        .Where(a => DateTime.ParseExact(a.Month.ToString("00") + "-" + a.Year, "MM-yyyy",
                                        CultureInfo.InvariantCulture, DateTimeStyles.None) >= period);
            }

            if (!string.IsNullOrWhiteSpace(toPeriod))
            {
                var result = DateTime.TryParseExact(toPeriod, "MM-yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var period);

                if (result)
                    climateStationReadingQuery = climateStationReadingQuery
                        .Where(a => DateTime.ParseExact(a.Month.ToString("00") + "-" + a.Year, "MM-yyyy",
                                        CultureInfo.InvariantCulture, DateTimeStyles.None) <= period);
            }

            climateStationReadingQuery = climateStationReadingQuery.OrderBy(u => u.Year).ThenBy(u => u.Month)
                .ThenBy(u => u.ClimateStation.Name);


            var paginatedReadings =
                PaginateList(pagination, climateStationReadingQuery.ProjectTo<ClimateStationReadingVM>()).ToList();

            var totalPages = GetTotalPages(pagination.PageSize, climateStationReadingQuery.Count());

            var climateStationReadingsModel = new ClimateStationReadingListVM
            {
                ClimateStationReadings = paginatedReadings,
                NextPage = pagination.ShowPage < totalPages ? pagination.ShowPage + 1 : pagination.ShowPage,
                PreviousPage = pagination.ShowPage > 1 ? pagination.ShowPage - 1 : pagination.ShowPage,
                CurrentPage = pagination.ShowPage,
                TotalPages = totalPages,
                ShowPagination = totalPages > 1
            };

            return View("../ClimateStationReadings/Index", climateStationReadingsModel);
        }

        [HttpGet]
        [Route("/readings/{climateStationId}/{month}/{year}")]
        public IActionResult ClimateStationReading(string climateStationId, int month, int year)
        {
            if (HasAlert) SetAlertModel();

            if (string.IsNullOrWhiteSpace(climateStationId)) return NotFound("invalid climate station id");

            var climateStationReading = _climateStationReadingService.GetAll()
                .Include(r => r.ClimateStation)
                .FirstOrDefault(r => r.ClimateStationId == climateStationId && r.Month == month && r.Year == year);

            if (climateStationReading == null) return NotFound("reading not found");

            var climateStationReadingModel = Mapper.Map<ClimateStationReadingFullVM>(climateStationReading);

            return View("../ClimateStationReadings/ClimateStationReading", climateStationReadingModel);
        }
    }
}