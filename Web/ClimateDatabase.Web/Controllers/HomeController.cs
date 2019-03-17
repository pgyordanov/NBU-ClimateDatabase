namespace ClimateDatabase.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ClimateDatabase.Web.Controllers.Base;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}