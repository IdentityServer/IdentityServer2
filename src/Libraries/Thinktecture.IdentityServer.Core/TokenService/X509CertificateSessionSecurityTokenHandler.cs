/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.TokenService
{
    public class X509CertificateSessionSecurityTokenHandler : SessionSecurityTokenHandler
    {
        public X509CertificateSessionSecurityTokenHandler(X509Certificate2 protectionCertificate)
            : base(CreateTransforms(protectionCertificate))
        { }

        private static ReadOnlyCollection<CookieTransform> CreateTransforms(X509Certificate2 protectionCertificate)
        {
            var transforms = new List<CookieTransform>() 
               { 
                 new DeflateCookieTransform(), 
                 new RsaEncryptionCookieTransform(protectionCertificate),
                 new RsaSignatureCookieTransform(protectionCertificate),
               };

            return transforms.AsReadOnly();
        }
    }
}
