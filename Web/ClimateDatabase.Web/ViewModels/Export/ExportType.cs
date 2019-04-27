namespace ClimateDatabase.Web.ViewModels.Export
{
    using System.ComponentModel;

    public enum ExportType
    {
        [Description("By station")]
        ByStation = 1,
        [Description("National data weighted")]
        ByPeriodWeighted
    }
}