using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        [Import]
        public IUserManagementRepository UserManagementRepository { get; set; }

        public RoleController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public RoleController(IUserManagementRepository userManagementRepository)
        {
            UserManagementRepository = userManagementRepository;
        }

        public ActionResult Index()
        {
            var vm = new RolesViewModel(UserManagementRepository);
            return View("Index", vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserManagementRepository.CreateRole(model.Name);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error creating role.");
                }
            }

            return Index();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RoleInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserManagementRepository.DeleteRole(model.Name);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error deleting role.");
                }
            }

            return Index();
        }
    }
}
