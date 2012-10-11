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
        public ActionResult Index(string action, RoleInputModel[] list)
        {
            if (action == "new") return Create();
            if (action == "delete") return Delete(list);
            return HttpNotFound();
        }

        public ActionResult Create()
        {
            return View("Create", new RoleInputModel());
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

            return View("Create", model);
        }

        private ActionResult Delete(RoleInputModel[] list)
        {
            var query = from item in list
                        where item.Delete && !(item.CanDelete)
                        select item.Name;
            foreach(var name in query)
            {
                ModelState.AddModelError("", "Can't delete role " + name + ".");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in list.Where(x=>x.Delete && x.CanDelete).Select(x=>x.Name))
                    {
                        UserManagementRepository.DeleteRole(item);
                    }
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
            
            var vm = new RolesViewModel(UserManagementRepository);
            return View("Index", vm);
        }
    }
}
