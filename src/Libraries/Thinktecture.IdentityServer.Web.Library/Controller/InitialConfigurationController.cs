/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class InitialConfigurationController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public InitialConfigurationController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public InitialConfigurationController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationRepository.SigningCertificate.SubjectDistinguishedName))
            {
                return RedirectToAction("index", "home");
            }

            var model = new InitialConfigurationModel
            {
                AvailableCertificates = GetAvailableCertificatesFromStore(),
                IssuerUri = ConfigurationRepository.Configuration.IssuerUri,
                SiteName = ConfigurationRepository.Configuration.SiteName
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InitialConfigurationModel model)
        {
            if (ModelState.IsValid)
            {
                var config = ConfigurationRepository.Configuration;
                config.SiteName = model.SiteName;
                config.IssuerUri = model.IssuerUri;

                ConfigurationRepository.UpdateConfiguration(config);
                ConfigurationRepository.UpdateCertificates(null, model.SigningCertificate);

                return RedirectToAction("index", "home");
            }

            ModelState.AddModelError("", "Errors ocurred...");
            model.AvailableCertificates = GetAvailableCertificatesFromStore();
            return View(model);
        }

        #region Helper
        private List<string> GetAvailableCertificatesFromStore()
        {
            var list = new List<string>();
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            try
            {
                foreach (var cert in store.Certificates)
                {
                    // todo: add friendly name
                    list.Add(string.Format("{0}", cert.Subject));
                }
            }
            finally
            {
                store.Close();
            }

            return list;
        }
        #endregion
    }
}
