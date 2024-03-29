﻿using System.Configuration;
using System.Web.Mvc;
using QiuYudengMathematics.Comm;

namespace QiuYudengMathematics.Filter
{
    public class RoleFilters : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (ConfigurationManager.AppSettings["adminAccount"].ToString() != WebSiteComm.CurrentUserAccount)
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["HomePageUrl"]);
                return;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}