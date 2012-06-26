/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Helper;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class FederationMetadataController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        [Import]
        public ICacheRepository CacheRepository { get; set; }

        public FederationMetadataController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public FederationMetadataController(IConfigurationRepository configurationRepository, ICacheRepository cacheRepository)
        {
            ConfigurationRepository = configurationRepository;
            CacheRepository = cacheRepository;
        }

        public ActionResult Generate()
        {
            if (ConfigurationRepository.Endpoints.FederationMetadata)
            {
                return Cache.ReturnFromCache<ActionResult>(CacheRepository, Constants.CacheKeys.WSFedMetadata, 1, () =>
                    {
                        var endpoints = Endpoints.Create(
                            HttpContext.Request.Headers["Host"],
                            HttpContext.Request.ApplicationPath,
                            ConfigurationRepository.Endpoints.HttpPort,
                            ConfigurationRepository.Endpoints.HttpsPort);

                        return new ContentResult
                        {
                            Content = new WSFederationMetadataGenerator(endpoints).Generate(),
                            ContentType = "text/xml"
                        };
                    });
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }
    }
}
