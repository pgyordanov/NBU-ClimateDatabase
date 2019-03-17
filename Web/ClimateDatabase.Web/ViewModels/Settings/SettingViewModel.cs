namespace ClimateDatabase.Web.ViewModels.Settings
{
    using ClimateDatabase.Common.Mapping;
    using ClimateDatabase.Data.Models;

    public class SettingViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
