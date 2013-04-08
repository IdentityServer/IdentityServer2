/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Thinktecture.IdentityModel.WSTrust;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class WSTrustClientFactory
    {
        public static IWSTrustChannelContract CreateMixedUserNameClient(string userName, string password, string endpointAddress)
        {
            var factory = new WSTrustChannelFactory(
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                new EndpointAddress(endpointAddress));
            factory.TrustVersion = TrustVersion.WSTrust13;

            factory.Credentials.UserName.UserName = userName;
            factory.Credentials.UserName.Password = password;

            return factory.CreateChannel();
        }

        public static IWSTrustChannelContract CreateMixedCertificateClient(string subjectName, string endpointAddress)
        {
            var factory = new WSTrustChannelFactory(
                new CertificateWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                new EndpointAddress(endpointAddress));
            factory.TrustVersion = TrustVersion.WSTrust13;

            factory.Credentials.ClientCertificate.SetCertificate(
                StoreLocation.CurrentUser,
                StoreName.My,
                X509FindType.FindBySubjectDistinguishedName,
                subjectName);

            return factory.CreateChannel();
        }
    }
}
