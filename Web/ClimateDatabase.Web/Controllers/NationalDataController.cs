using System;
using System.Collections.Generic;
using System.Linq;
using ClimateDatabase.Services.Contracts;
using ClimateDatabase.Services.Models;
using ClimateDatabase.Web.Controllers.Base;
using ClimateDatabase.Web.ViewModels.NationalData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClimateDatabase.Web.Controllers
{
    public class NationalDataController : BaseController
    {
        private readonly IClimateDataService _climateDataService;

        public NationalDataController(IClimateDataService climateData)
        {
            _climateDataService = climateData;
        }

        [HttpGet]
        [Route("/national")]
        public IActionResult Index()
        {
            IEnumerable<SelectListItem> climateFields = Enum.GetValues(typeof(ClimateDataField))
                .Cast<ClimateDataField>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int) v).ToString()
                }).ToList();

            climateFields.First().Selected = true;

            return View(new NationalDataChartViewModel
            {
                climateFields = climateFields,
                climateData = FetchNationalData(0, DateTime.Now.AddYears(-1), DateTime.Now)
            });
        }


        [HttpGet]
        [Route("/national/fetch")]
        public JsonResult FetchNationalData(int fieldId, DateTime from, DateTime to)
        {
            var nationalWeightedData = _climateDataService.GetWeightedDataForPeriodByField(new ClimateDataFilter
            {
                ClimateDataField = (ClimateDataField) fieldId,
                From = from,
                To = to
            });

            return new JsonResult(nationalWeightedData);
        }
    }
}