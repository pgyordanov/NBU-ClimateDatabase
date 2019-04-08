using System.Collections.Generic;
using ClimateDatabase.Data.Models;
using ClimateDatabase.Web.ViewModels.Base;

namespace ClimateDatabase.Web.Models
{
    public class ClimateStationReadingListVM : PaginatedWithMappingVM<ClimateStationReading>
    {
        public List<ClimateStationReadingVM> ClimateStationReadings { get; set; }
    }
}