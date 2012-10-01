/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Helper
{
    /// <summary>
    /// Helper class to retrieve certificates from configuration
    /// </summary>
    public static class X509Certificates
    {
       
        /// <summary>
        /// Retrieves a certificate from the certificate store.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="name">The name.</param>
        /// <param name="findType">Type of the find.</param>
        /// <param name="value">The value.</param>
        /// <returns>A X509Certificate2</returns>
        public static X509Certificate2 GetCertificateFromStore(StoreLocation location, StoreName name, X509FindType findType, object value)
        {
            X509Store store = new X509Store(name, location);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                // work around possible bug in framework
                if (findType == X509FindType.FindByThumbprint)
                {
                    var thumbprint = value.ToString();
                    thumbprint = thumbprint.Trim();
                    thumbprint = thumbprint.Replace(" ", "");

                    foreach (var cert in store.Certificates)
                    {
                        if (string.Equals(cert.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(cert.Thumbprint, thumbprint, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return cert;
                        }
                    }
                }
                if (findType == X509FindType.FindBySerialNumber)
                {
                    var serial = value.ToString();
                    serial = serial.Trim();
                    serial = serial.Replace(" ", "");

                    foreach (var cert in store.Certificates)
                    {
                        if (string.Equals(cert.SerialNumber, serial, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(cert.SerialNumber, serial, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return cert;
                        }
                    }
                }

                var certs = store.Certificates.Find(findType, value, false);

                if (certs.Count != 1)
                {
                    throw new InvalidOperationException(String.Format("Certificate not found: {0}", value));
                }

                return certs[0];
            }
            finally
            {
                store.Close();
            }
        }

        

        /// <summary>
        /// Retrieves a certificate from the local machine / personal certificate store.
        /// </summary>
        /// <param name="subjectDistinguishedName">The subject distinguished name of the certificate.</param>
        /// <returns>A X509Certificate2</returns>
        public static X509Certificate2 GetCertificateFromStore(string subjectDistinguishedName)
        {
            return GetCertificateFromStore(
                StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindBySubjectDistinguishedName,
                subjectDistinguishedName);
        }
    }
}
