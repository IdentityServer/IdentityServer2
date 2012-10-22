/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.TokenService
{
    public class SignInSessionsManager
    {
        private const string COOKIENAME = ".idsrvsso";
        
        HttpContextBase _context;
        int _maximumCookieLifetime;

        public SignInSessionsManager(HttpContextBase context) : this(context, 24)
        { }

        public SignInSessionsManager(HttpContextBase context, int maximumCookieLifetime)
        {
            _context = context;
            _maximumCookieLifetime = maximumCookieLifetime;
        }

        public void AddRealm(string realm)
        {
            var realms = ReadCookie();
            if (!realms.Contains(realm.ToLowerInvariant()))
            {
                realms.Add(realm.ToLowerInvariant());
                WriteCookie(realms);
            }
        }

        public List<string> GetRealms()
        {
            return ReadCookie();
        }

        public void ClearRealms()
        {
            var cookie = _context.Request.Cookies[COOKIENAME];
            if (cookie != null)
            {
                cookie.Value = "";
                cookie.Expires = new DateTime(2000, 1, 1);
                _context.Response.SetCookie(cookie);
            }
        }

        private List<string> ReadCookie()
        {
            var cookie = _context.Request.Cookies[COOKIENAME];
            if (cookie == null)
            {
                return new List<string>();
            }

            return cookie.Value.Split('|').ToList();
        }

        private void WriteCookie(List<string> realms)
        {
            if (realms.Count == 0)
            {
                ClearRealms();
                return;
            }

            var realmString = string.Join("|", realms).ToLowerInvariant();

            var cookie = new HttpCookie(COOKIENAME, realmString)
            {
                Expires = DateTime.Now.AddHours(_maximumCookieLifetime),
                HttpOnly = true,
                Path = HttpRuntime.AppDomainAppVirtualPath
            };

            _context.Response.Cookies.Add(cookie);
        }
    }
}
