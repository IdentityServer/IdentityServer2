///*
// * Copyright (c) Dominick Baier.  All rights reserved.
// * see license.txt
// */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.IdentityModel.Protocols.WSTrust;
//using System.IdentityModel.Tokens;
//using System.IO;
//using System.Security.Claims;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using System.Xml;
//using Thinktecture.IdentityServer.Repositories;
//using Thinktecture.IdentityServer.Web.Security;
//using Thinktecture.IdentityServer.Web.ViewModels.Administration;

//namespace Thinktecture.IdentityServer.Web.Controllers
//{
//    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
//    public class AdminController : Controller
//    {
//        [Import]
//        public IUserRepository UserRepository { get; set; }

//        [Import]
//        public IClaimsRepository ClaimsRepository { get; set; }

//        [Import]
//        public IConfigurationRepository ConfigurationRepository { get; set; }

//        public AdminController()
//        {
//            Container.Current.SatisfyImportsOnce(this);
//        }

//        public AdminController(IUserRepository userRepository, IConfigurationRepository configurationRepository, IClaimsRepository claimsRepository)
//        {
//            UserRepository = userRepository;
//            ClaimsRepository = claimsRepository;
//            ConfigurationRepository = configurationRepository;
//        }

//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult MyClaims()
//        {
//            var myModel = new MyModel
//            {
//                Claims = GetClaims()
//            };

//            return View(myModel);
//        }

//        public ActionResult MyToken()
//        {
//            var config = ConfigurationRepository.Global;
//            var samlHandler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection()[config.DefaultWSTokenType];
            
//            var descriptor = new SecurityTokenDescriptor
//            {
//                AppliesToAddress = "http://self",
//                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(config.DefaultTokenLifetime)),
//                SigningCredentials = new X509SigningCredentials(ConfigurationRepository.Keys.SigningCertificate),
//                TokenIssuerName = config.IssuerUri,
//                Subject = new ClaimsIdentity(GetClaims())
//            };

//            var token = samlHandler.CreateToken(descriptor);

//            var sb = new StringBuilder(1024);
//            samlHandler.WriteToken(XmlWriter.Create(new StringWriter(sb)), token);

//            return new ContentResult
//            {
//                ContentType = "text/xml",
//                Content = sb.ToString()
//            };
//        }

//        public ActionResult Configuration()
//        {
//            ViewBag.SaveEnabled = ConfigurationRepository.SupportsWriteAccess;
//            return View(ConfigurationRepository.Configuration.ToViewModel());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Configuration(GlobalConfigurationModel configuration)
//        {
//            if (ModelState.IsValid)
//            {
//                if (ConfigurationRepository.SupportsWriteAccess)
//                {
//                    ConfigurationRepository.UpdateConfiguration(configuration.ToDomainModel());
//                }
                
//                return RedirectToAction("Index");
//            }

//            return View();
//        }

//        public ActionResult Endpoints()
//        {
//            ViewBag.SaveEnabled = ConfigurationRepository.SupportsWriteAccess;
//            return View(ConfigurationRepository.Endpoints.ToViewModel());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Endpoints(EndpointConfigurationModel endpoints)
//        {
//            if (ModelState.IsValid)
//            {
//                if (ConfigurationRepository.SupportsWriteAccess)
//                {
//                    ConfigurationRepository.UpdateEndpoints(endpoints.ToDomainModel());
//                }
                
//                return RedirectToAction("Index");
//            }

//            return View();
//        }

//        [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.ServiceCertificates)]
//        public ActionResult Certificates()
//        {
//            var model = new EditCertificatesModel
//            {
//                AvailableCertificates = GetAvailableCertificatesFromStore(),
                
//                SigningCertificate = ConfigurationRepository.SigningCertificate.SubjectDistinguishedName,
//                SslCertificate = ConfigurationRepository.SslCertificate.SubjectDistinguishedName
//            };

//            ViewBag.SaveEnabled = ConfigurationRepository.SupportsWriteAccess;
//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.ServiceCertificates)]
//        public ActionResult Certificates(EditCertificatesModel model)
//        {
//            string newSsl = null, newSigning = null;

//            if (model.UpdateSslCertificate)
//            {
//                newSsl = model.UpdatedSslCertificate;
//            }
//            if (model.UpdateSigningCertificate)
//            {
//                newSigning = model.UpdatedSigningCertificate;
//            }

//            if (ConfigurationRepository.SupportsWriteAccess)
//            {
//                ConfigurationRepository.UpdateCertificates(newSsl, newSigning);
//            }

//            return RedirectToAction("Index");
//        }

//        public ActionResult Restart()
//        {
//            return View("Confirm");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Restart(FormCollection collection)
//        {
//            HttpRuntime.UnloadAppDomain();
            
//            return RedirectToAction("Index");
//        }

//        #region Helper
//        private List<Claim> GetClaims()
//        {
//            return TokenService.TokenService.GetOutputClaims(HttpContext.User as ClaimsPrincipal, null, ClaimsRepository);
//        }

//        private List<string> GetAvailableCertificatesFromStore()
//        {
//            var list = new List<string>();
//            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
//            store.Open(OpenFlags.ReadOnly);

//            try
//            {
//                foreach (var cert in store.Certificates)
//                {
//                    list.Add(cert.Subject);
//                }
//            }
//            finally
//            {
//                store.Close();
//            }

//            return list;
//        }
//        #endregion
//    }
//}
