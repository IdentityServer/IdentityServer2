/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Web.Mvc;
using Thinktecture.IdentityModel;
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
            if (ConfigurationRepository.Keys.SigningCertificate != null)
            {
                return RedirectToAction("index", "home");
            }

            var model = new InitialConfigurationModel
            {
                AvailableCertificates = GetAvailableCertificatesFromStore(),
                IssuerUri = ConfigurationRepository.Global.IssuerUri,
                SiteName = ConfigurationRepository.Global.SiteName
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InitialConfigurationModel model)
        {
            if (ConfigurationRepository.Keys.SigningCertificate != null)
            {
                return RedirectToAction("index", "home");
            }

            if (ModelState.IsValid)
            {
                var config = ConfigurationRepository.Global;
                config.SiteName = model.SiteName;
                config.IssuerUri = model.IssuerUri;

                // update global config
                ConfigurationRepository.Global = config;

                var keys = ConfigurationRepository.Keys;
                try
                {
                    var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(model.SigningCertificate).First();
                    
                    // make sure we can access the private key
                    var pk = cert.PrivateKey;
                    
                    keys.SigningCertificate = cert;
                }
                catch (CryptographicException)
                {
                    ModelState.AddModelError("", WindowsIdentity.GetCurrent().Name + " does not have read access to the private key of the signing certificate you selected (see http://technet.microsoft.com/en-us/library/ee662329.aspx).");
                    model.AvailableCertificates = GetAvailableCertificatesFromStore();
                    return View(model);
                }


                
                // updates key material config
                ConfigurationRepository.Keys = keys;

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
