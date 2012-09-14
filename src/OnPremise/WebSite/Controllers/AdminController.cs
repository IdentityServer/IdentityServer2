using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class AdminController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }
        [Import]
        public IRelyingPartyRepository RelyingPartyRepository { get; set; }

        public AdminController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public AdminController(IConfigurationRepository configuration, IRelyingPartyRepository relyingPartyRepository)
        {
            ConfigurationRepository = configuration;
            RelyingPartyRepository = relyingPartyRepository;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult General()
        {
            var model = ConfigurationRepository.Global;
            return View("General", model);
        }
        
        [HttpPost]
        public ActionResult General(GlobalConfiguration model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ConfigurationRepository.Global = model;
                    return RedirectToAction("General");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating configuration.");
                }
            }

            return View("General", model);
        }

        bool IsTopRequestFor(params string[] actions)
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
        public ActionResult Protocols_Navigation()
        {
            if (IsTopRequestFor("Protocols", "Protocol"))
            {
                var vm = new ProtocolsViewModel(ConfigurationRepository);
                var list = vm.Protocols.Where(x => x.Enabled).ToArray();
                if (list.Length > 0)
                {
                    return PartialView("_Protocols_Navigation", list);
                }
            }

            return new EmptyResult();
        }

        public ActionResult Protocols()
        {
            var vm = new ProtocolsViewModel(ConfigurationRepository);
            return View("Protocols", vm);
        }
        
        [HttpPost]
        public ActionResult Protocols(ProtocolsInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Update(this.ConfigurationRepository);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating protocols.");
                }
                return RedirectToAction("Protocols");
            }

            var vm = new ProtocolsViewModel(ConfigurationRepository);
            return View("Protocols", vm);
        }

        public ActionResult Protocol(string id)
        {
            var vm = new ProtocolsViewModel(this.ConfigurationRepository);
            var protocol = vm.GetProtocol(id);
            if (protocol == null) return HttpNotFound();
            return View("Protocol", protocol);
        }

        [HttpPost]
        public ActionResult UpdateProtocol(string id)
        {
            var vm = new ProtocolsViewModel(this.ConfigurationRepository);
            var protocol = vm.GetProtocol(id);
            if (this.TryUpdateModel(protocol.Protocol, "protocol"))
            {
                try
                {
                    vm.UpdateProtocol(protocol);
                    return RedirectToAction("Protocol", new { id });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating protocol.");
                }
            }
            return View("Protocol", protocol);
        }

        bool TryUpdateModel(object model, string prefix)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var valueProvider = this.ValueProvider;
            
            Predicate<string> predicate = delegate(string propertyName)
            {
                return IsPropertyAllowed(propertyName, null, null);
            };
            var modelType = model.GetType();
            IModelBinder binder = this.Binders.GetBinder(modelType);
            ModelBindingContext context2 = new ModelBindingContext();
            context2.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(delegate
            {
                return model;
            }, modelType);
            context2.ModelName = prefix;
            context2.ModelState = this.ModelState;
            context2.PropertyFilter = predicate;
            context2.ValueProvider = valueProvider;
            ModelBindingContext bindingContext = context2;
            binder.BindModel(base.ControllerContext, bindingContext);
            return this.ModelState.IsValid;
        }

        internal static bool IsPropertyAllowed(string propertyName, string[] includeProperties, string[] excludeProperties)
        {
            bool flag = ((includeProperties == null) || (includeProperties.Length == 0)) || includeProperties.Contains<string>(propertyName, StringComparer.OrdinalIgnoreCase);
            bool flag2 = (excludeProperties != null) && excludeProperties.Contains<string>(propertyName, StringComparer.OrdinalIgnoreCase);
            return (flag && !flag2);
        }

        [ChildActionOnly]
        public ActionResult RPs_Navigation()
        {
            if (IsTopRequestFor("RPs"))
            {
                var list = RelyingPartyRepository.List(-1, -1);
                if (list.Any())
                {
                    return PartialView("_RPs_Navigation", list);
                }
            }
            return new EmptyResult();
        }

        public ActionResult RPs()
        {
            return View();
        }

    }
}
