/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Security;
using Thinktecture.IdentityServer.Web.ViewModels.Administration;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Delegation)]
    public class DelegationAdminController : Controller
    {
        [Import]
        public IDelegationRepository Repository { get; set; }

        public DelegationAdminController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public DelegationAdminController(IDelegationRepository repository)
        {
            Repository = repository;
        }

        public ActionResult Index()
        {
            var users = Repository.GetAllUsers(-1, -1);

            return View(users);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddDelegationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var setting = new DelegationSetting
            {
                UserName = model.UserName,
                Realm = new Uri(model.Realm),
                Description = model.Description
            };

            try
            {
                Repository.Add(setting);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message);
                return View("Add");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string userName)
        {
            var model = new EditDelegationModel
            {
                UserName = userName
            };

            model.Settings = Repository.GetDelegationSettingsForUser(userName);

            if (model.Settings.ToList().Count == 0)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult DeleteAll(string userName)
        {
            return View("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll(string userName, FormCollection collection)
        {
            try
            {
                Repository.GetDelegationSettingsForUser(userName).ToList().ForEach(setting =>
                    Repository.Delete(new DelegationSetting { UserName = userName, Realm = setting.Realm }));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message);
                return View("Delete");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string userName, string thumbprint)
        {
            return View("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string userName, string realm, FormCollection collection)
        {
            try
            {
                Repository.Delete(new DelegationSetting { UserName = userName, Realm = new Uri(realm) });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message ?? ex.Message);
                return View("Delete");
            }

            return RedirectToAction("Edit", new { userName = userName });
        }
    }
}
