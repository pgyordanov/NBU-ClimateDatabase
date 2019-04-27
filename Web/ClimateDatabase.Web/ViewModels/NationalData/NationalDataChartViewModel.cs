using System.Collections.Generic;
using ClimateDatabase.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace ClimateDatabase.Web.ViewModels.NationalData
{
    public class NationalDataChartViewModel
    {
        public IEnumerable<SelectListItem> climateFields { get; set; }
        
        public JsonResult climateData { get; set; }
    }
}