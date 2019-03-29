namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationReading
{
    using System.Collections.Generic;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.Base;

    public class ClimateStationReadingListVM : PaginatedWithMappingVM<ClimateStationReading>
    {
       public List<ClimateStationReadingVM> ClimateStationReadings { get; set; }
    }
}
