/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Helper
{
    public static class Cache
    {
        public static T ReturnFromCache<T>(ICacheRepository cacheRepository, string name, int ttl, Func<T> action)   
            where T: class
        {
            var item = cacheRepository.Get(name) as T;
            if (item == null)
            {
                item = action();
                cacheRepository.Put(name, item, ttl);
            }

            return item;
        }
    }
}
