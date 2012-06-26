/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.Composition.Hosting;

namespace Thinktecture.IdentityServer.Repositories
{
    public static class Container
    {
        public static CompositionContainer Current { get; set; }
    }
}