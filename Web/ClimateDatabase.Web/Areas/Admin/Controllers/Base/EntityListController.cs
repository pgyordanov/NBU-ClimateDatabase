namespace ClimateDatabase.Web.Areas.Admin.Controllers.Base
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ClimateDatabase.Common;
    using ClimateDatabase.Web.Areas.Admin.Models;

    public class EntityListController : BaseController
    {
        protected IQueryable<T> PaginateList<T>(PaginationVM pagination, IQueryable<T> query)
        {
            var skip = (pagination.ShowPage - 1) * pagination.PageSize;
            var take = pagination.PageSize;

            return query.Skip(skip).Take(take);
        }

        protected int GetTotalPages(int pageSize, int entityCount)
        {
            var totalPages = (int)Math.Ceiling(decimal.Divide(entityCount, pageSize));

            return totalPages;
        }

        protected PaginationVM GetCurrentPagination()
        {
            PaginationVM pagination = new PaginationVM
            {
                ShowPage = !string.IsNullOrWhiteSpace(this.Request.Query["showPage"]) ? int.Parse(this.Request.Query["showPage"]) : 1,
                PageSize = !string.IsNullOrWhiteSpace(this.Request.Query["pageSize"]) ? int.Parse(this.Request.Query["pageSize"]) : 20,
            };

            return pagination;
        }
    }
}