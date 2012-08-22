/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels.Administration;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.ClientCertificates)]
    public class ClientCertificatesAdminController : Controller
    {
        [Import]
        public IClientCertificatesRepository Repository { get; set; }

        public ClientCertificatesAdminController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public ClientCertificatesAdminController(IClientCertificatesRepository repository)
        {
            Repository = repository;
        }

        public ActionResult Index()
        {
            var users = Repository.List(-1, -1);

            return View(users);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddClientCertificateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var clientCert = new ClientCertificate
            {
                UserName = model.UserName,
                Description = model.Description
            };

            if (model.CertificateUpload != null && model.CertificateUpload.ContentLength > 0)
            {
                var bytes = new byte[model.CertificateUpload.InputStream.Length];
                model.CertificateUpload.InputStream.Read(bytes, 0, bytes.Length);

                clientCert.Thumbprint = new X509Certificate2(bytes).Thumbprint;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.Thumbprint))
                {
                    ModelState.AddModelError("", "No certificate (or thumbprint) specified");
                    return View();
                }

                clientCert.Thumbprint = model.Thumbprint;
            }

            try
            {
                Repository.Add(clientCert);
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
            var model = new EditClientCertificatesModel
            {
                UserName = userName
            };

            model.ClientCertificates = Repository.GetClientCertificatesForUser(userName);

            if (model.ClientCertificates.ToList().Count == 0)
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
                Repository.GetClientCertificatesForUser(userName).ToList().ForEach(cert =>
                    Repository.Delete(new ClientCertificate { UserName = userName, Thumbprint = cert.Thumbprint }));
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
        public ActionResult Delete(string userName, string thumbprint, FormCollection collection)
        {
            try
            {
                Repository.Delete(new ClientCertificate { UserName = userName, Thumbprint = thumbprint });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message);
                return View("Delete");
            }

            return RedirectToAction("Edit", new { userName = userName });
        }
    }
}
