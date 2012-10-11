using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    public class KeyConfigurationController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public KeyConfigurationController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public KeyConfigurationController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        public ActionResult Index()
        {
            var keys = this.ConfigurationRepository.Keys;
            var vm = new KeyConfigurationViewModel(keys);
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(KeyConfigurationInputModel keys)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    keys.Update(this.ConfigurationRepository);
                    TempData["Message"] = "Update Successful";
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating keys.");
                }
            }

            var vm = new KeyConfigurationViewModel(this.ConfigurationRepository.Keys);
            vm.Keys = keys;
            return View("Index", vm);
        }
    }
}
