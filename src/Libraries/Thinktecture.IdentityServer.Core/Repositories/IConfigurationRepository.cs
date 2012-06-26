/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IConfigurationRepository
    {
        GlobalConfiguration Configuration { get; }
        EndpointConfiguration Endpoints { get; }
        CertificateConfiguration SslCertificate { get; }
        CertificateConfiguration SigningCertificate { get; }

        bool SupportsWriteAccess { get; }
        void UpdateConfiguration(GlobalConfiguration configuration);
        void UpdateEndpoints(EndpointConfiguration endpoints);
        void UpdateCertificates(string sslSubjectName, string signingSubjectName);
    }
}