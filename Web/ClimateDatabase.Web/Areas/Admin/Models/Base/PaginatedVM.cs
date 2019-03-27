namespace ClimateDatabase.Web.Areas.Admin.Models.Base
{
    public class PaginatedVM
    {
        public int CurrentPage { get; set; }

        public int NextPage { get; set; }

        public int PreviousPage { get; set; }

        public int TotalPages { get; set; }

        public bool ShowPagination { get; set; }
    }
}