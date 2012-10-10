using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    public class DelegationController : Controller
    {
        [Import]
        public IDelegationRepository delegationRepository { get; set; }
        
        public DelegationController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public DelegationController(IDelegationRepository delegationRepository)
        {
            this.delegationRepository = delegationRepository;
        }

        public ActionResult Index()
        {
            var vm = new DelegationViewModel(this.delegationRepository);
            return View("Index", vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, DelegationUser[] users)
        {
            if (action == "new") return Configure();
            if (action == "delete") return Delete(users);
            return RedirectToAction("Index");
        }

        private ActionResult Delete(DelegationUser[] users)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var user in users.Where(x => x.Delete))
                    {
                        var settings = this.delegationRepository.GetDelegationSettingsForUser(user.Username);
                        foreach (var setting in settings)
                        {
                            this.delegationRepository.Delete(setting);
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error deleting delegation setting.");
                }
            }

            return Index();
        }

        public ActionResult Configure(string id = null)
        {
            var vm = new DelegationSettingsForUserInputModel(this.delegationRepository, id);
            return View("Configure", vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Configure(string action, DelegationSettingsForUserInputModel model)
        {
            if (action == "create") return Create(model);
            if (action == "save") return Save(model);
            
            return RedirectToAction("Index");
        }

        private ActionResult Create(DelegationSettingsForUserInputModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.Settings)
                {
                    
                }
                return RedirectToAction("Index");
            }

            return Configure(model.UserName);
        }

        private ActionResult Save(DelegationSettingsForUserInputModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            
            return Configure(model.UserName);
        }
    }
}
