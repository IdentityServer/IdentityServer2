/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    /// <summary>
    /// Repository for handling refresh tokens
    /// </summary>
    public interface ICodeTokenRepository
    {
        string AddCode(CodeTokenType type, int clientId, string userName, string scope);
        bool TryGetCode(string tokenIdentifier, out CodeToken token);
        void DeleteCode(string tokenIdentifier);
    }
}
