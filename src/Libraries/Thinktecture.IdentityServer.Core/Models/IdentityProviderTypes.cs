/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;
namespace Thinktecture.IdentityServer.Models
{
    public enum IdentityProviderTypes
    {
        WSStar = 1,
        //public const string OpenId = "OpenId";
        OAuth2 = 2
    }
}
