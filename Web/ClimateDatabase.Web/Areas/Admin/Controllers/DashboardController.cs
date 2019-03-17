namespace ClimateDatabase.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ClimateDatabase.Common;
    using ClimateDatabase.Web.Areas.Admin.Controllers.Base;

    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}