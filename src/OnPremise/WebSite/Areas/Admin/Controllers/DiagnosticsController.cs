using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class DiagnosticsController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public DiagnosticsController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public DiagnosticsController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        public ActionResult Index()
        {
            var vm = this.ConfigurationRepository.Diagnostics;
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DiagnosticsConfiguration model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ConfigurationRepository.Diagnostics = model;
                    TempData["Message"] = "Update Successful";
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating diagnostics.");
                }
            }

            return View("Index", model);
        }

    }
}
