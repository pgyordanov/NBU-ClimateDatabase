namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStation
{
    using System.Collections.Generic;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.Base;

    public class ClimateStationListVM : PaginatedWithMappingVM<ClimateStation>
    {
       public List<ClimateStationVM> ClimateStations { get; set; }
    }
}
