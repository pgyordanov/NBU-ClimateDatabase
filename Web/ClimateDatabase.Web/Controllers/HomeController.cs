namespace ClimateDatabase.Web.Controllers
{
    using ClimateDatabase.Web.Controllers.Base;
    using ClimateDatabase.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "ClimateStationReadings", new PaginationVM { ShowPage = 1, PageSize = 20 });

            // return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}