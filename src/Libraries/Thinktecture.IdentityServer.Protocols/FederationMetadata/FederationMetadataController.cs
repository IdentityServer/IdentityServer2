/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Helper;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.FederationMetadata
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
            if (ConfigurationRepository.FederationMetadata.Enabled)
            {
                return Cache.ReturnFromCache<ActionResult>(CacheRepository, Constants.CacheKeys.WSFedMetadata, 1, () =>
                    {
                        var host = ConfigurationRepository.Global.PublicHostName;
                        if (String.IsNullOrWhiteSpace(host))
                        {
                            host = HttpContext.Request.Headers["Host"];
                        }
                        var endpoints = Endpoints.Create(
                            host,
                            HttpContext.Request.ApplicationPath,
                            ConfigurationRepository.Global.HttpPort,
                            ConfigurationRepository.Global.HttpsPort);

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
