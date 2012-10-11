using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    [MetadataType(typeof(KeyMaterialConfiguration))]
    public class KeyConfigurationInputModel
    {
        public KeyConfigurationInputModel()
        {
        }
        public KeyConfigurationInputModel(Models.Configuration.KeyMaterialConfiguration keysConfig)
        {
            if (keysConfig.SigningCertificate != null)
            {
                SigningCertificate = keysConfig.SigningCertificate.Subject;
            }
            if (keysConfig.DecryptionCertificate != null)
            {
                DecryptionCertificate = keysConfig.DecryptionCertificate.Subject;
            }

            SymmetricSigningKey = keysConfig.SymmetricSigningKey;
        }

        public string SigningCertificate { get; set; }
        public string DecryptionCertificate { get; set; }
        public string SymmetricSigningKey { get; set; }

        public void Update(Repositories.IConfigurationRepository ConfigurationRepository)
        {
            var keys = ConfigurationRepository.Keys;
            try
            {
                var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(SigningCertificate, false).First();

                // make sure we can access the private key
                var pk = cert.PrivateKey;

                keys.SigningCertificate = cert;
            }
            catch (CryptographicException)
            {
                throw new ValidationException(WindowsIdentity.GetCurrent().Name + " does not have read access to the private key of the signing certificate you selected (see http://technet.microsoft.com/en-us/library/ee662329.aspx).");
            }

            if (DecryptionCertificate == null)
            {
                keys.DecryptionCertificate = null;
            }
            else
            {
                try
                {
                    var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(DecryptionCertificate, false).First();

                    // make sure we can access the private key
                    var pk = cert.PrivateKey;

                    keys.DecryptionCertificate = cert;
                }
                catch (CryptographicException)
                {
                    throw new ValidationException(WindowsIdentity.GetCurrent().Name + " does not have read access to the private key of the signing certificate you selected (see http://technet.microsoft.com/en-us/library/ee662329.aspx).");
                }
            }

            if (string.IsNullOrWhiteSpace(SymmetricSigningKey))
            {
                keys.SymmetricSigningKey = Convert.ToBase64String(CryptoRandom.CreateRandomKey(32));
            }
            else
            {
                keys.SymmetricSigningKey = SymmetricSigningKey;
            }

            // updates key material config
            ConfigurationRepository.Keys = keys;
        }
    }

    public class KeyConfigurationViewModel
    {
        public IEnumerable<SelectListItem> AllCerts
        {
            get
            {
                var allCerts = GetAvailableCertificatesFromStore().Select(x => new SelectListItem { Text = x }).ToList();
                allCerts.Insert(0, new SelectListItem { Text = "-Choose-", Value = "" });
                return allCerts;
            }
        }

        public KeyConfigurationInputModel Keys { get; set; }

        public KeyConfigurationViewModel(Models.Configuration.KeyMaterialConfiguration config)
        {
            this.Keys = new KeyConfigurationInputModel(config);
        }

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
    }
}