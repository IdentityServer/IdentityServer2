using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
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
            if (IsTopRequestFor("RPs", "RP"))
            {
                var vm = new RelyingPartiesViewModel(RelyingPartyRepository);
                var list = vm.RPs.Where(x=>x.Enabled);
                if (list.Any())
                {
                    return PartialView("_RPs_Navigation", list);
                }
            }
            return new EmptyResult();
        }

        public ActionResult RPs()
        {
            var vm = new RelyingPartiesViewModel(RelyingPartyRepository);
            return View("RPs", vm);
        }
        
        [HttpPost]
        public ActionResult RPs(string action, IEnumerable<RelyingPartyViewModel> list)
        {
            if (action == "save")
            {
                var vm = new RelyingPartiesViewModel(RelyingPartyRepository);
                if (ModelState.IsValid)
                {
                    vm.Update(list);
                    return RedirectToAction("RPs");
                }

                return View("RPs", vm);
            }

            if (action == "new")
            {
                return RedirectToAction("RP");
            }

            ModelState.AddModelError("", "Invalid action.");
            return View("RPs", new RelyingPartiesViewModel(RelyingPartyRepository));
        }

        public ActionResult RP(string id)
        {
            RelyingParty rp = null;
            if (id == null) rp = new RelyingParty();
            else rp = this.RelyingPartyRepository.Get(id);
            if (rp == null) return HttpNotFound();
            return View("RP", rp);
        }

        [HttpPost]
        public ActionResult RP(string id, 
            string action,
            [Bind(Exclude = "EncryptingCertificate")] RelyingParty rp, 
            RPCertInputModel cert)
        {
            if (action == "create")
            {
                return CreateRP(rp, cert);
            }
            if (action == "save")
            {
                return SaveRP(id, rp, cert);
            }
            if (action == "delete")
            {
                return DeleteRP(id);
            }
            
            var origRP = this.RelyingPartyRepository.Get(id);
            rp.EncryptingCertificate = origRP.EncryptingCertificate;
            
            ModelState.AddModelError("", "Invalid action.");
            return View("RP", rp);
        }

        private ActionResult DeleteRP(string id)
        {
            try
            {
                this.RelyingPartyRepository.Delete(id);
                return RedirectToAction("RPs");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch
            {
                ModelState.AddModelError("", "Error updating relying party.");

            }
            
            var rp = this.RelyingPartyRepository.Get(id);
            return View("RP", rp);
        }

        private ActionResult CreateRP(RelyingParty rp, RPCertInputModel cert)
        {
            // ID is not required for create
            ModelState["ID"].Errors.Clear();

            rp.Id = null;
            if (cert.EncryptingCertificate != null && cert.EncryptingCertificate.ContentLength > 0)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        cert.EncryptingCertificate.InputStream.CopyTo(ms);
                        var bytes = ms.ToArray();
                        var val = new X509Certificate2(bytes);
                        rp.EncryptingCertificate = val;
                    }
                }
                catch
                {
                    ModelState.AddModelError("EncryptingCertificate", "Error processing certificate.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.RelyingPartyRepository.Add(rp);
                    return RedirectToAction("RPs");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating relying party.");
                }
            }

            return View("RP", rp);
        }

        private ActionResult SaveRP(string id, RelyingParty rp, RPCertInputModel cert)
        {
            if (cert.RemoveCert == true)
            {
                rp.EncryptingCertificate = null;
            }
            else if (cert.EncryptingCertificate != null && cert.EncryptingCertificate.ContentLength > 0)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        cert.EncryptingCertificate.InputStream.CopyTo(ms);
                        var bytes = ms.ToArray();
                        var val = new X509Certificate2(bytes);
                        rp.EncryptingCertificate = val;
                    }
                }
                catch
                {
                    ModelState.AddModelError("EncryptingCertificate", "Error processing certificate.");
                }
            }
            else
            {
                var origRP = this.RelyingPartyRepository.Get(id);
                rp.EncryptingCertificate = origRP.EncryptingCertificate;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.RelyingPartyRepository.Update(rp);
                    return RedirectToAction("RP", new { id });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating relying party.");
                }
            }

            return View("RP", rp);
        }

    }
}
