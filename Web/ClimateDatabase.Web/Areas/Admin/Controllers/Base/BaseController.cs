﻿namespace ClimateDatabase.Web.Areas.Admin.Controllers.Base
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ClimateDatabase.Common;
    using ClimateDatabase.Web.Areas.Admin.Models;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Admin")]
    public class BaseController : Controller
    {
        private const string HasAlertKey = "hasAlert";
        private const string AlertMessageKey = "alertMessage";
        private const string AlertTypeKey = "alertType";

        protected bool HasAlert
        {
            get
            {
                return this.TempData[HasAlertKey] != null ? (bool)this.TempData[HasAlertKey] : false;
            }

            set
            {
                this.TempData[HasAlertKey] = value;
            }
        }

        protected void AddAlert(bool success, string message)
        {
            this.TempData[AlertMessageKey] = message;
            this.TempData[AlertTypeKey] = success;

            this.HasAlert = true;
        }

        protected void SetAlertModel()
        {
            AlertVM alert = new AlertVM { Message = (string)this.TempData[AlertMessageKey], Success = (bool)this.TempData[AlertTypeKey] };
            this.ViewBag.Alert = alert;

            this.TempData.Remove(HasAlertKey);
            this.TempData.Remove(AlertMessageKey);
            this.TempData.Remove(AlertTypeKey);
        }
    }
}
