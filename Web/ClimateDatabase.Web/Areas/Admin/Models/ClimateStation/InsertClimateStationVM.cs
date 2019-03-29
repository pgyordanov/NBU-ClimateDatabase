namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStation
{
    using System.ComponentModel.DataAnnotations;

    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class InsertClimateStationVM : IMapFrom<ClimateStation>
    {
        [Required]
        public string Name { get; set; }
    }
}
