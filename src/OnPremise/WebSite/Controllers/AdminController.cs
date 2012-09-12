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
            var config = ConfigurationRepository.Global;
            var vm = new AdminConfigurationGeneralViewModel(config);
            return View("General", vm);
        }
        
        [HttpPost]
        public ActionResult General(AdminConfigurationGeneralViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ConfigurationRepository.Global = model.ToModel();
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

    }
}
