using ClimateDatabase.Common.Extensions;
using ClimateDatabase.Web.ViewModels.NationalData;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClimateDatabase.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;
    using ClimateDatabase.Services.Models;
    using ClimateDatabase.Web.ViewModels.Export;
    using Microsoft.AspNetCore.Mvc;

    [Route("export")]
    public class ExportController : Controller
    {
        private readonly IClimateDataService _climateData;

        public ExportController(IClimateDataService climateData)
        {
            this._climateData = climateData;
        }

        [HttpGet]
        [Route("data.csv")]
        [Produces("text/csv")]
        public async Task<IActionResult> GetDataAsCsv([FromQuery] ExportFilter filter)
        {
            var climateDataFilter = new ClimateDataFilter()
            {
                From = filter.From,
                To = filter.To
            };

            if (filter.ExportType != ExportType.ByPeriodWeighted)
            {
                List<ClimateDataModelByStation> climateDataByStation = await this._climateData.GetClimateDataByStation(climateDataFilter);
                return this.Ok(climateDataByStation);
            }

            if (filter.ClimateDataField == null)
            {
                List<ClimateDataModelByDate> weightedClimateDataForPeriodFull = this._climateData.GetWeightedClimateDataForPeriodFull(climateDataFilter);
                return this.Ok(weightedClimateDataForPeriodFull);
            }

            climateDataFilter.ClimateDataField = filter.ClimateDataField.Value;
            var weightedFieldByPeriod = this._climateData.GetWeightedDataForPeriodByField(climateDataFilter)
                .Select(
                    x => new
                    {
                        Date = x.Key,
                        x.Value
                    })
                .ToList();

            return this.Ok(weightedFieldByPeriod);
        }

        public IActionResult Index()
        {
            IEnumerable<SelectListItem> climateFields = Enum.GetValues(typeof(ClimateDataField))
                .Cast<ClimateDataField>()
                .Select(
                    v => new SelectListItem
                    {
                        Text = v.GetDescription(),
                        Value = ((int)v).ToString()
                    }).ToList();

            climateFields.First().Selected = true;

            IEnumerable<SelectListItem> exportTypes = Enum.GetValues(typeof(ExportType))
                .Cast<ExportType>()
                .Select(
                    v => new SelectListItem
                    {
                        Text = v.GetDescription(),
                        Value = ((int)v).ToString()
                    }).ToList();

            exportTypes.First(x => x.Value == ((int)ExportType.ByPeriodWeighted).ToString()).Selected = true;

            return View(
                new ExportIndexVM()
                {
                    ClimateFields = climateFields,
                    ExportType = exportTypes
                });
        }
    }
}