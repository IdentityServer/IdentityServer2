/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;

namespace Thinktecture.IdentityServer.Repositories
{
    public static class StandardClaimTypes
    {
        static Dictionary<string, string> _mappings;

        public static Dictionary<string, string> Mappings
        {
            get { return _mappings; }
        }

        static StandardClaimTypes()
        {
            _mappings = new Dictionary<string, string>
            {
                { "country", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country" },
                { "dateofBirth", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth" },
                { "email", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" },
                { "gender", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender" },
                { "givenname", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname" },
                { "homephone", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone" },
                { "locality", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality" },
                { "mobilephone", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone" },
                { "otherphone", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone" },
                { "postalcode", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode" },
                { "stateorprovince", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince" },
                { "streetaddress", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress" },
                { "surname", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname" },
                { "webpage" ,"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage" }
            };
        }
    }
}
