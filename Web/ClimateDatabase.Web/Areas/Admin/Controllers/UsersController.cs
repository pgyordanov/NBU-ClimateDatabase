namespace ClimateDatabase.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Identity;
    using ClimateDatabase.Web.Areas.Admin.Controllers.Base;
    using ClimateDatabase.Web.Areas.Admin.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.Users;

    public class UsersController : EntityListController
    {
        private ApplicationUserManager<ApplicationUser> userManager;

        public UsersController(ApplicationUserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("admin/users")]
        public IActionResult Index(PaginationVM pagination, string usernameEmail)
        {
            string currentId = this.userManager.GetUserId(this.User);
            var usersQuery = this.userManager.Users.Where(u => u.Id != currentId);

            if (!string.IsNullOrWhiteSpace(usernameEmail))
            {
                usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(usernameEmail.ToLower()) || u.Email.ToLower().Contains(usernameEmail.ToLower()));
            }

            usersQuery = usersQuery.OrderBy(u => u.IsDeleted).ThenByDescending(u => u.CreatedOn);

            var paginatedUsers = this.PaginateList<UserVM>(pagination, usersQuery.ProjectTo<UserVM>()).ToList();

            int totalPages = this.GetTotalPages(pagination.PageSize, usersQuery.Count());

            return this.View(new UserListVM
            {
                Users = paginatedUsers,
                NextPage = pagination.ShowPage < totalPages ? pagination.ShowPage + 1 : pagination.ShowPage,
                PreviousPage = pagination.ShowPage > 1 ? pagination.ShowPage - 1 : pagination.ShowPage,
                CurrentPage = pagination.ShowPage,
                TotalPages = totalPages,
                ShowPagination = totalPages > 1,
            });
        }

        [HttpGet]
        [Route("admin/user/{userId}")]
        public IActionResult UserProfile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.NotFound($"invalid user id");
            }

            var user = this.userManager.Users.Where(u => u.Id == userId).ProjectTo<UserVM>().FirstOrDefault();

            if (user == null)
            {
                return this.NotFound($"user not found");
            }

            if (this.HasAlert)
            {
                this.SetAlertModel();
            }

            return this.View(user);
        }

        [HttpPost]
        [Route("admin/user/{userId}/activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.NotFound($"invalid user id");
            }

            IdentityResult result = await this.userManager.ActivateUserAsync(userId);

            this.AddAlert(true, "User account successfully activated");

            return this.RedirectToAction("UserProfile", "Users", new { userId });
        }

        [HttpPost]
        [Route("admin/user/{userId}/deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.NotFound($"invalid user id");
            }

            IdentityResult result = await this.userManager.DeactivateUserAsync(userId);

            this.AddAlert(true, "User account successfully deactivated");

            return this.RedirectToAction("UserProfile", "Users", new { userId });
        }
    }
}