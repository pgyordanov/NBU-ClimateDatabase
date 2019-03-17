namespace ClimateDatabase.Web.Areas.Admin.Models.Users
{
    using System.Collections.Generic;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.Base;

    public class UserListVM : PaginatedWithMappingVM<ApplicationUser>
    {
        public List<UserVM> Users { get; set; }
    }
}