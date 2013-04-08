/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Tests
{
    class TestRelyingPartyRepository : IRelyingPartyRepository
    {
        List<RelyingParty> _rps = new List<RelyingParty>
        {
            new RelyingParty
            {
                Id = "1",
                Enabled = true,
                Name = "Plain text, no encryption",
                Realm = new Uri(Constants.Realms.PlainTextNoEncryption)
            },
            new RelyingParty
            {
                Id = "2",
                Enabled = true,
                Name = "SSL, no encryption",
                Realm = new Uri(Constants.Realms.SslNoEncryption)
            },
            new RelyingParty
            {
                Id = "3",
                Enabled = true,
                Name = "Plain text, encryption",
                Realm = new Uri(Constants.Realms.PlainTextEncryption),
                EncryptingCertificate = Constants.Certificates.DefaultEncryptionCertificate
            },
            new RelyingParty
            {
                Id = "4",
                Enabled = true,
                Name = "SSL, encryption",
                Realm = new Uri(Constants.Realms.SslEncryption),
                EncryptingCertificate = Constants.Certificates.DefaultEncryptionCertificate
            },
            new RelyingParty
            {
                Id = "4",
                Enabled = true,
                Name = "Explicit replyTo",
                Realm = new Uri(Constants.Realms.ExplicitReplyTo),
                ReplyTo = new Uri(Constants.Realms.ExplicitReplyTo),
                EncryptingCertificate = Constants.Certificates.DefaultEncryptionCertificate
            },
            new RelyingParty
            {
                Id = "5",
                Enabled = false,
                Name = "Disabled RP",
                Realm = new Uri(Constants.Realms.DisabledRP),
                EncryptingCertificate = Constants.Certificates.DefaultEncryptionCertificate
            }
        };

        public bool TryGet(string realm, out RelyingParty relyingParty)
        {
            relyingParty =
                (from rp in _rps
                 where rp.Realm.AbsoluteUri.Equals(realm, StringComparison.OrdinalIgnoreCase)
                 select rp)
                .FirstOrDefault();

            return (relyingParty != null);
        }

        #region Management - not implemented
        public bool SupportsWriteAccess
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<Models.RelyingParty> List(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Models.RelyingParty Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Add(Models.RelyingParty relyingParty)
        {
            throw new NotImplementedException();
        }

        public void Update(Models.RelyingParty relyingParty)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
