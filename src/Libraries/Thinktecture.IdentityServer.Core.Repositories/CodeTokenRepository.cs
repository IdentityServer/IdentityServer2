/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class CodeTokenRepository : ICodeTokenRepository
    {
        public string AddCode(CodeTokenType type, int clientId, string userName, string scope)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var code = Guid.NewGuid().ToString("N");

                var refreshToken = new CodeToken
                {
                    Type = (int)type,
                    Code = code,
                    ClientId = clientId,
                    Scope = scope,
                    UserName = userName,
                    TimeStamp = DateTime.UtcNow
                };

                entities.CodeTokens.Add(refreshToken);
                entities.SaveChanges();

                return code;
            }
        }

        public bool TryGetCode(string code, out Models.CodeToken token)
        {
            token = null;

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var entity = (from t in entities.CodeTokens
                              where t.Code.Equals(code, StringComparison.OrdinalIgnoreCase)
                              select t)
                             .FirstOrDefault();

                if (entity != null)
                {
                    token = entity.ToDomainModel();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void DeleteCode(string code)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.CodeTokens.Where(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (item != null)
                {
                    entities.CodeTokens.Remove(item);
                    entities.SaveChanges();
                }
            }
        }

        public IEnumerable<Models.CodeToken> Search(int? clientId, string username, string scope, CodeTokenType type)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var query = 
                    from t in entities.CodeTokens
                    where t.Type == (int)type
                    select t;

                if (clientId != null)
                {
                    query =
                        from t in query
                        where t.ClientId == clientId.Value
                        select t;
                }

                if (!String.IsNullOrWhiteSpace(username))
                {
                    query =
                        from t in query
                        where t.UserName.Contains(username)
                        select t;
                }

                if (!String.IsNullOrWhiteSpace(scope))
                {
                    query =
                        from t in query
                        where t.Scope.Contains(scope)
                        select t;
                }

                var results = query.ToArray().Select(x => x.ToDomainModel());
                return results;
            }
        }
    }
}
