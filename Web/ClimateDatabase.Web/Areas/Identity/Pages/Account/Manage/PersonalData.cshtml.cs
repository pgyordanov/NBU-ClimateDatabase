﻿namespace ClimateDatabase.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Identity;

#pragma warning disable SA1649 // File name should match first type name
    public class PersonalDataModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly ApplicationUserManager<ApplicationUser> userManager;
        private readonly ILogger<PersonalDataModel> logger;

        public PersonalDataModel(
            ApplicationUserManager<ApplicationUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            return this.Page();
        }
    }
}
