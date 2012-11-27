using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class MenuController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public MenuController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public MenuController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var vm = new MenuViewModel(this.ConfigurationRepository);
            return PartialView("Index", vm);
        }

        bool IsController(string controller)
        {
            ControllerContext ctx = this.ControllerContext;
            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }
            var val = (string)ctx.RouteData.Values["controller"];
            return controller.Equals(val, StringComparison.OrdinalIgnoreCase);
        }
        
        bool IsAction(params string[] actions)
        {
            ControllerContext ctx = this.ControllerContext;
            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }
            var action = (string)ctx.RouteData.Values["action"];
            return actions.Contains(action, StringComparer.OrdinalIgnoreCase);
        }

        [ChildActionOnly]
        public ActionResult ChildMenu(string controllerName)
        {
            if (IsController(controllerName))
            {
                return PartialView("LoadChildMenu", controllerName);
            }

            return new EmptyResult();
        }
    }
}
