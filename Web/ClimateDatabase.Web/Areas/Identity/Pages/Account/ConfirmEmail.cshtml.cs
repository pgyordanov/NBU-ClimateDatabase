﻿namespace ClimateDatabase.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Identity;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class ConfirmEmailModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly ApplicationUserManager<ApplicationUser> userManager;

        public ConfirmEmailModel(ApplicationUserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userId}'.");
            }

            if (user.EmailConfirmed)
            {
                throw new InvalidOperationException("email already confirmed");
            }

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return this.Page();
        }
    }
}
