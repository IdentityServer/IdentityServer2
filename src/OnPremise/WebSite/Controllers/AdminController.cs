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

        public AdminController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public AdminController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
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
        
        public ActionResult RPs()
        {
            return View();
        }

    }
}
