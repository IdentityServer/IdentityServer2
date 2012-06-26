/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.IdentityModel.Tokens;

namespace Thinktecture.IdentityServer.TokenService
{
    class ClientCertificateIssuerNameRegistry : IssuerNameRegistry
    {
        public override string GetIssuerName(SecurityToken securityToken)
        {
            if (securityToken == null)
            {
                Tracing.Error("ClientCertificateIssuerNameRegistry: securityToken is null");
                throw new ArgumentNullException("securityToken");
            }

            X509SecurityToken token = securityToken as X509SecurityToken;
            if (token != null)
            {
                Tracing.Verbose("ClientCertificateIssuerNameRegistry: X509 SubjectName: " + token.Certificate.SubjectName.Name);
                Tracing.Verbose("ClientCertificateIssuerNameRegistry: X509 Thumbprint : " + token.Certificate.Thumbprint);
                return token.Certificate.Thumbprint;
            }

            return null;
        }
    }
}
