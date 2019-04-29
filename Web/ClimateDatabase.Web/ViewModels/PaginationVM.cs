namespace ClimateDatabase.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class PaginationVM
    {
        // Default arguments are needed to avoid case where all attributes are = 0
        public PaginationVM()
        {
            this.ShowPage = 1;
            this.PageSize = 20;
        }

        [Range(1, 40)]
        public int PageSize { get; set; }

        [Range(1, int.MaxValue)]
        public int ShowPage { get; set; }
    }
}