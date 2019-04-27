using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClimateDatabase.Web.ViewModels.Export
{
    public class ExportIndexVM
    {
        public IEnumerable<SelectListItem> ClimateFields { get; set; }

        public IEnumerable<SelectListItem> ExportType { get; set; }
    }
}