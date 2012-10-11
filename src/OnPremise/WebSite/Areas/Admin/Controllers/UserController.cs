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
    public class UserController : Controller
    {
        [Import]
        public IUserManagementRepository UserManagementRepository { get; set; }

        public UserController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public UserController(IUserManagementRepository userManagementRepository)
        {
            UserManagementRepository = userManagementRepository;
        }

        public ActionResult Index(string filter = null)
        {
            var vm = new UsersViewModel(UserManagementRepository, filter);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, UserDeleteModel[] list)
        {
            if (action == "new") return Create();
            if (action == "delete") return Delete(list);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View("Create", new UserInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.UserManagementRepository.CreateUser(model.Username, model.Password, model.Email);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error creating user.");
                }                
            }
            return View("Create", model);
        }

        private ActionResult Delete(UserDeleteModel[] list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var name in list.Where(x=>x.Delete).Select(x=>x.Username))
                    {
                        this.UserManagementRepository.DeleteUser(name);
                    }
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error deleting user.");
                }
            }
            return Index();
        }

        public ActionResult Roles(string id)
        {
            var vm = new UserRolesViewModel(this.UserManagementRepository, id);
            return View("Roles", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Roles(string id, UserRoleAssignment[] roleAssignments)
        {
            var vm = new UserRolesViewModel(this.UserManagementRepository, id);
            if (ModelState.IsValid)
            {
                var currentRoles = 
                    roleAssignments.Where(x=>x.InRole).Select(x=>x.Role);
                this.UserManagementRepository.SetRolesForUser(id, currentRoles);
                return RedirectToAction("Roles", new { id });
            }
            
            return View("Roles", vm);
        }
    }
}
