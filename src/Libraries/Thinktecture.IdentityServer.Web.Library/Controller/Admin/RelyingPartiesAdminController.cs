/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.Security.Cryptography;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels.Administration;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.RelyingParty)]
    public class RelyingPartiesAdminController : Controller
    {
        [Import]
        public IRelyingPartyRepository Repository { get; set; }

        public RelyingPartiesAdminController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public RelyingPartiesAdminController(IRelyingPartyRepository repository)
        {
            Repository = repository;
        }

        public ActionResult Index()
        {
            var rps = Repository.List(-1, -1);
            return View(rps.ToViewModel());
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(RelyingPartyModel relyingParty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TrySetCertificateFromUpload(relyingParty);
                    Repository.Add(relyingParty.ToDomainModel());

                    return RedirectToAction("Index");
                }

                return View(relyingParty);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(relyingParty);
            }
        }

        public ActionResult Edit(string id)
        {
            var rp = Repository.Get(id);
            return View(rp.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RelyingPartyModel relyingParty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TrySetCertificateFromUpload(relyingParty);
                    Repository.Update(relyingParty.ToDomainModel());

                    return RedirectToAction("Index");
                }

                return View(relyingParty);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                }

                return View(relyingParty);
            }
        }

        public ActionResult Delete(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                Repository.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult GenerateSymmetricSigningKey()
        {
            var bytes = new byte[32];
            new RNGCryptoServiceProvider().GetBytes(bytes);

            return Json(Convert.ToBase64String(bytes));
        }

        private void TrySetCertificateFromUpload(RelyingPartyModel relyingParty)
        {
            if (relyingParty.CertificateUpload != null && relyingParty.CertificateUpload.ContentLength > 0)
            {
                byte[] bytes = new byte[relyingParty.CertificateUpload.InputStream.Length];
                relyingParty.CertificateUpload.InputStream.Read(bytes, 0, bytes.Length);

                relyingParty.EncryptingCertificate = Convert.ToBase64String(bytes);
            }
        }
    }
}
