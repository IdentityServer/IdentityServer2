/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.Composition;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.GlobalFilter
{
    public class GlobalViewModelFilter : ActionFilterAttribute
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Container.Current.SatisfyImportsOnce(this);

            filterContext.Controller.ViewBag.SiteName = ConfigurationRepository.Global.SiteName;
            filterContext.Controller.ViewBag.IsAdministrator = ClaimsAuthorization.CheckAccess(Constants.Actions.Administration, Constants.Resources.UI);
            filterContext.Controller.ViewBag.IsSignedIn = filterContext.HttpContext.User.Identity.IsAuthenticated;

            base.OnActionExecuting(filterContext);
        }
    }
}