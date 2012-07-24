///*
// * Copyright (c) Dominick Baier.  All rights reserved.
// * see license.txt
// */

//using System.ComponentModel.Composition;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using AutoMapper;
//using Thinktecture.IdentityServer.Helper;
//using Thinktecture.IdentityServer.Models;

//namespace Thinktecture.IdentityServer.Repositories.Sql
//{
//    public class ConfigurationRepository : IConfigurationRepository
//    {
//        private const string StandardConfigurationName = "Standard";
//        private const string SslCertificateName = "SSL";
//        private const string SigningCertificateName = "SigningCertificate";

//        private const string EndpointConfigurationCacheKey = "Cache_EndpointConfiguration";
//        private const string GlobalConfigurationCacheKey = "Cache_GlobalConfiguration";
//        private const string SslCertificateCachekey = "Cache_SslCertificate";
//        private const string SigningCertificateCacheKey = "Cache_SigningCertificate";

//        [Import]
//        public ICacheRepository CacheRepository { get; set; }

//        static ConfigurationRepository()
//        {
//            Mapper.CreateMap<GlobalConfiguration, Global>()
//                .ForMember(m => m.Name, opt => opt.Ignore());

//            Mapper.CreateMap<Global, GlobalConfiguration>();

//            Mapper.CreateMap<EndpointConfiguration, Endpoints>()
//                .ForMember(m => m.Name, opt => opt.Ignore());

//            Mapper.CreateMap<Endpoints, EndpointConfiguration>();

//            Mapper.AssertConfigurationIsValid();
//        }

//        public ConfigurationRepository()
//        {
//            Container.Current.SatisfyImportsOnce(this);
//        }

//        public ConfigurationRepository(ICacheRepository cacheRepository)
//        {
//            CacheRepository = cacheRepository;
//        }

//        #region Runtime
//        public GlobalConfiguration Configuration
//        {
//            get
//            {
//                return Cache.ReturnFromCache<GlobalConfiguration>(CacheRepository, GlobalConfigurationCacheKey, 1, () =>
//                {
//                    using (var entities = IdentityServerConfigurationContext.Get())
//                    {
//                        var global = (from c in entities.Global where c.Name == StandardConfigurationName select c).FirstOrDefault();
//                        if (global == null)
//                        {
//                            throw new ConfigurationErrorsException("No standard global configuration found");
//                        }

//                        return Mapper.Map<Global, GlobalConfiguration>(global);
//                    }
//                });
//            }
//        }

//        public EndpointConfiguration Endpoints
//        {
//            get
//            {
//                return Cache.ReturnFromCache<EndpointConfiguration>(CacheRepository, EndpointConfigurationCacheKey, 1, () =>
//                {
//                    using (var entities = IdentityServerConfigurationContext.Get())
//                    {
//                        var eps = (from c in entities.Endpoints where c.Name == StandardConfigurationName select c).FirstOrDefault();
//                        if (eps == null)
//                        {
//                            throw new ConfigurationErrorsException("No standard endpoint configuration found in database");
//                        }

//                        return Mapper.Map<Endpoints, EndpointConfiguration>(eps);
//                    }
//                });
//            }
//        }

//        public CertificateConfiguration SslCertificate
//        {
//            get
//            {
//                return Cache.ReturnFromCache<CertificateConfiguration>(CacheRepository, SslCertificateCachekey, 1, () =>
//                {
//                    using (var entities = IdentityServerConfigurationContext.Get())
//                    {
//                        var cert = (from c in entities.Certificates where c.Name == SslCertificateName select c).FirstOrDefault();
//                        if (cert == null)
//                        {
//                            throw new ConfigurationErrorsException("No SSL certificate found in database");
//                        }

//                        return LoadCertificateConfiguration(cert);
//                    }
//                });
//            }
//        }

//        public CertificateConfiguration SigningCertificate
//        {
//            get
//            {
//                return Cache.ReturnFromCache<CertificateConfiguration>(CacheRepository, SigningCertificateCacheKey, 1, () =>
//                {
//                    using (var entities = IdentityServerConfigurationContext.Get())
//                    {
//                        var cert = (from c in entities.Certificates where c.Name == SigningCertificateName select c).FirstOrDefault();
//                        if (cert == null)
//                        {
//                            throw new ConfigurationErrorsException("No signing certificate found in database");
//                        }

//                        return LoadCertificateConfiguration(cert);
//                    }
//                });
//            }
//        }

//        protected virtual CertificateConfiguration LoadCertificateConfiguration(Certificates cert)
//        {
//            object findValue;
//            X509FindType findType;

//            var certConfig = new CertificateConfiguration
//            {
//                SubjectDistinguishedName = cert.SubjectDistinguishedName,
//            };

//            if (!string.IsNullOrWhiteSpace(cert.SubjectDistinguishedName))
//            {
//                findValue = cert.SubjectDistinguishedName;
//                findType = X509FindType.FindBySubjectDistinguishedName;
//            }
//            else
//            {
//                Tracing.Error("No distinguished name or thumbprint for certificate: " + cert.Name);
//                return certConfig;
//            }

//            try
//            {
//                certConfig.Certificate = X509Certificates.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.My, findType, findValue);
//            }
//            catch
//            {
//                Tracing.Error("No certificate found for: " + findValue);
//                throw new ConfigurationErrorsException("No certificate found for: " + findValue);
//            }

//            return certConfig;
//        }
//        #endregion

//        #region Management
//        public bool SupportsWriteAccess
//        {
//            get { return true; }
//        }

//        public void UpdateConfiguration(GlobalConfiguration configuration)
//        {
//            using (var entities = IdentityServerConfigurationContext.Get())
//            {
//                var entity = Mapper.Map<GlobalConfiguration, Global>(configuration);
//                entity.Name = StandardConfigurationName;

//                entities.Global.Attach(entity);
//                entities.Entry(entity).State = EntityState.Modified;

//                entities.SaveChanges();
//                CacheRepository.Invalidate(GlobalConfigurationCacheKey);
//                CacheRepository.Invalidate(Constants.CacheKeys.WSFedMetadata);
//            }
//        }

//        public void UpdateEndpoints(EndpointConfiguration endpoints)
//        {
//            using (var entities = IdentityServerConfigurationContext.Get())
//            {
//                var entity = Mapper.Map<EndpointConfiguration, Endpoints>(endpoints);
//                entity.Name = StandardConfigurationName;

//                entities.Endpoints.Attach(entity);
//                entities.Entry(entity).State = System.Data.EntityState.Modified;

//                entities.SaveChanges();
//                CacheRepository.Invalidate(EndpointConfigurationCacheKey);
//                CacheRepository.Invalidate(Constants.CacheKeys.WSFedMetadata);
//            }
//        }

//        public void UpdateCertificates(string sslSubjectName, string signingSubjectName)
//        {
//            using (var entities = IdentityServerConfigurationContext.Get())
//            {
//                var certs = entities.Certificates;

//                if (!string.IsNullOrWhiteSpace(sslSubjectName))
//                {
//                    var ssl = new Certificates
//                    {
//                        Name = SslCertificateName,
//                        SubjectDistinguishedName = sslSubjectName
//                    };

//                    certs.Attach(ssl);
//                    entities.Entry(ssl).State = EntityState.Modified;
//                }

//                if (!string.IsNullOrWhiteSpace(signingSubjectName))
//                {
//                    var signing = new Certificates
//                    {
//                        Name = SigningCertificateName,
//                        SubjectDistinguishedName = signingSubjectName
//                    };

//                    certs.Attach(signing);
//                    entities.Entry(signing).State = EntityState.Modified;
//                }

//                entities.SaveChanges();

//                CacheRepository.Invalidate(SigningCertificateCacheKey);
//                CacheRepository.Invalidate(SslCertificateCachekey);
//                CacheRepository.Invalidate(Constants.CacheKeys.WSFedMetadata);
//            }
//        }
//        #endregion
//    }
//}
