namespace ClimateDatabase.Web.Areas.Admin.Models.Base
{
    using ClimateDatabase.Common.Mapping;

    public class PaginatedWithMappingVM<T> : PaginatedVM, IMapFrom<T>
    {
    }
}