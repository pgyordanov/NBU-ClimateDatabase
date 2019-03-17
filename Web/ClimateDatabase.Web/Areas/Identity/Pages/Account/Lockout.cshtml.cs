namespace ClimateDatabase.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using ClimateDatabase.Common.Exceptions;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Identity;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class LockoutModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly ApplicationUserManager<ApplicationUser> userManager;

        public LockoutModel(ApplicationUserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnGetAsync(string userId)
        {
            ApplicationUser user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException("userId");
            }

            if (!user.LockoutEnd.HasValue || DateTimeOffset.UtcNow.Subtract(user.LockoutEnd.Value).TotalMilliseconds <= 0)
            {
                throw new InvalidOperationException("email is confirmed");
            }
        }
    }
}
